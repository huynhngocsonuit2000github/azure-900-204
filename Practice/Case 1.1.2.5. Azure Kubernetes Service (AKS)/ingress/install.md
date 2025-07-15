# create namespace
kubectl create namespace ingress-nginx

# Apply Official NGINX Ingress YAML
kubectl apply -f https://raw.githubusercontent.com/kubernetes/ingress-nginx/controller-v1.10.1/deploy/static/provider/cloud/deploy.yaml

# Wait for the External IP
kubectl get svc ingress-nginx-controller -n ingress-nginx


# Problem
You installed the NGINX Ingress Controller in AKS, but the EXTERNAL-IP for its LoadBalancer service was stuck in <pending>
Kubernetes was unable to automatically assign a public IP to your LoadBalancer service because:
- Azure sometimes does not auto-create public IPs when:
    - You're out of quota, OR
    - You want to use a pre-created static IP (recommended for production)

# Get the public IP in the Azure subscription
az network public-ip list --query "[?ipAddress!=null]" --output table
List all public IPs in your Azure subscription
So we could:

Check if any usable static IPs already exist
Find their names and which resource group they belong to
Attach one of them manually to your Ingress

Location    Name                                  IdleTimeoutInMinutes    IpAddress       ProvisioningState    PublicIPAddressVersion    PublicIPAllocationMethod    ResourceGuid                          ResourceGroup
----------  ------------------------------------  ----------------------  --------------  -------------------  ------------------------  --------------------------  ------------------------------------  ----------------------------
westus      my-jenkins-mv-ip                      4                       20.228.105.117  Succeeded            IPv4                      Static                      dc977be8-f7ff-44a0-9025-d77b42dc5d09  ask-rg
westus      37bb5577-705d-49b5-8443-790cc834f025  4                       52.234.0.105    Succeeded            IPv4                      Static                      7dc1aae1-eaa7-45e4-8fb9-903598a90485  MC_ask-rg_aks-cluster_westus