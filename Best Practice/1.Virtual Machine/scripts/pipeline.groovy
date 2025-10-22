pipeline {
  agent any

  parameters {
    string(
      name: 'BRANCH',
      defaultValue: 'az-vmss-dotnet-v3',
      description: 'Git branch or tag to deploy'
    )
  }

  environment {
    // ==== EDIT TO MATCH YOUR ENV ====
    RG        = 'vmss-dotnet-rg'
    VMSS      = 'vmss-dotnet-api'
    APP_DIR   = '/app/mysimpleapi'
    SERVICE   = 'mysimpleapi'
    REPO_URL  = 'https://github.com/huynhngocsonuit2000github/az-packages.git'

    // Azure credentials (Jenkins Secret Texts)
    AZ_SUBSCRIPTION_ID = credentials('az-subscription-id')
    AZ_TENANT_ID       = credentials('az-tenant-id')
    AZ_CLIENT_ID       = credentials('az-client-id')
    AZ_CLIENT_SECRET   = credentials('az-client-secret')

    // Optional for private GitHub repos:
    // GITHUB_PAT = credentials('github-pat')
  }

  options {
    timestamps()
  }

  stages {

    stage('Prep: Tooling') {
      steps {
        sh '''
          set -e
          command -v az >/dev/null 2>&1 || { echo "Azure CLI not found. Install 'az' on the agent." >&2; exit 1; }
          command -v python3 >/dev/null 2>&1 || { echo "python3 not found. Install python3 on the agent." >&2; exit 1; }

          # envsubst is handy for templating the deploy script
          if ! command -v envsubst >/dev/null 2>&1; then
            if command -v apt-get >/dev/null 2>&1; then
              sudo apt-get update -y
              sudo apt-get install -y gettext-base
            else
              echo "envsubst not found. Install gettext on the agent." >&2
              exit 1
            fi
          fi
        '''
      }
    }

    stage('Azure Login') {
      steps {
        sh '''
          set -e
          az logout || true
          az cloud set -n AzureCloud
          az login --service-principal \
            --username "$AZ_CLIENT_ID" \
            --password "$AZ_CLIENT_SECRET" \
            --tenant   "$AZ_TENANT_ID" \
            --only-show-errors 1>/dev/null

          az account set --subscription "$AZ_SUBSCRIPTION_ID"
          echo "Logged in to Azure subscription: $(az account show --query name -o tsv)"
        '''
      }
    }

    stage('Ensure Manual Upgrade Mode') {
      steps {
        sh '''
          set -e
          MODE=$(az vmss show -g "$RG" -n "$VMSS" --query "upgradePolicy.mode" -o tsv || true)
          if [ "$MODE" != "Manual" ]; then
            echo "Switching upgradePolicy.mode to Manual (current: $MODE)"
            az vmss update -g "$RG" -n "$VMSS" --set upgradePolicy.mode=Manual 1>/dev/null
          else
            echo "Upgrade mode already Manual"
          fi
        '''
      }
    }

    stage('Create deploy.sh (template)') {
      steps {
        sh '''
          set -e

          # --- 1) Create a readable deploy script template ---
          cat > /tmp/deploy.sh.tmpl <<'BASH'
#!/usr/bin/env bash
set -euo pipefail

REPO_URL="${REPO_URL}"
BRANCH="${BRANCH}"
APP_DIR="${APP_DIR}"
SERVICE="${SERVICE}"

# If private repo, you can tokenize:
# REPO_URL="https://${GITHUB_PAT}@github.com/huynhngocsonuit2000github/az-packages.git"

echo "[APT] Update + deps"
sudo apt-get update -y
sudo apt-get install -y git unzip ca-certificates >/dev/null
sudo update-ca-certificates || true

echo "[GIT] Clone ${REPO_URL} @ ${BRANCH}"
sudo rm -rf /opt/az-packages
sudo git clone --depth 1 --branch "${BRANCH}" --single-branch "${REPO_URL}" /opt/az-packages

echo "[APP] Replace ${APP_DIR}"
sudo mkdir -p "${APP_DIR}"
sudo rm -rf "${APP_DIR:?}"/*
sudo unzip -o /opt/az-packages/publish.zip -d "${APP_DIR}" >/dev/null

echo "[SYSTEMD] Restart ${SERVICE}"
sudo systemctl daemon-reload || true
sudo systemctl restart "${SERVICE}"

echo "branch: ${BRANCH}" | sudo tee "${APP_DIR}/RELEASE" >/dev/null
echo "[DONE] Deployed ${BRANCH}"
BASH

          # --- 2) Fill placeholders with envsubst ---
          export REPO_URL="$REPO_URL"
          export BRANCH="$BRANCH"
          export APP_DIR="$APP_DIR"
          export SERVICE="$SERVICE"
          # If using PAT: export GITHUB_PAT="$GITHUB_PAT"

          envsubst < /tmp/deploy.sh.tmpl > /tmp/deploy.sh
          chmod +x /tmp/deploy.sh

          echo "[INFO] /tmp/deploy.sh created:"
          head -n 40 /tmp/deploy.sh
        '''
      }
    }

    stage('Build CSE JSON (embed deploy.sh)') {
      steps {
        sh '''
          set -e
          test -f /tmp/deploy.sh || { echo "/tmp/deploy.sh is missing"; exit 1; }

          B64=$(base64 -w0 /tmp/deploy.sh 2>/dev/null || base64 /tmp/deploy.sh)

          cat > /tmp/cse.json <<JSON
{
  "commandToExecute": "bash -lc 'set -euo pipefail; \
    echo ${B64} | base64 -d | sudo tee /tmp/deploy.sh >/dev/null; \
    sudo chmod +x /tmp/deploy.sh; \
    sudo -E REPO_URL=${REPO_URL} BRANCH=${BRANCH} APP_DIR=${APP_DIR} SERVICE=${SERVICE} /tmp/deploy.sh'"
}
JSON

          echo "[OK] wrote /tmp/cse.json"
          command -v jq >/dev/null && jq . /tmp/cse.json || true
        '''
      }
    }

    stage('Validate JSON before apply (hard stop on error)') {
      steps {
        sh '''
          set -e
          python3 - <<'PY'
import json
with open("/tmp/cse.json") as f:
    j = json.load(f)
cmd = j.get("commandToExecute", "")
assert cmd.startswith("bash -lc"), f"unexpected cmd: {cmd}"
assert "/tmp/deploy.sh" in cmd, "deploy.sh not executed in commandToExecute"
print("[OK] CSE JSON looks sane.")
PY
        '''
      }
    }

    stage('Update VMSS Model with CSE (force re-run)') {
      steps {
        sh '''
          set -e
          FORCE_TAG=$(date +%s)
          az vmss extension set \
            --resource-group "$RG" \
            --vmss-name "$VMSS" \
            --publisher Microsoft.Azure.Extensions \
            --name CustomScript \
            --version 2.1 \
            --protected-settings @/tmp/cse.json \
            --force-update

          echo "VMSS model updated (forceUpdate via flag) at ${FORCE_TAG}"
        '''
      }
    }

    stage('Redeploy to ALL instances') {
      steps {
        sh '''
          set -e
          az vmss update-instances \
            --resource-group "$RG" \
            --name "$VMSS" \
            --instance-ids "*"
          echo "Triggered model apply for all instances"
        '''
      }
    }

    stage('Verify') {
      steps {
        sh '''
          set -e
          echo "[VERIFY] VMSS instance summary:"
          az vmss list-instances -g "$RG" -n "$VMSS" \
            --query "[].{name:name, provisioning:provisioningState, latestModel:latestModelApplied}" -o table

           echo "[VERIFY] Extension summary from model:"
          az vmss show -g "$RG" -n "$VMSS" \
            --query "virtualMachineProfile.extensionProfile.extensions[].{name:name, type:type, version:typeHandlerVersion}" -o table
        '''
      }
    }
  }

  post {
    always {
      echo 'Pipeline finished.'
    }
  }
}
