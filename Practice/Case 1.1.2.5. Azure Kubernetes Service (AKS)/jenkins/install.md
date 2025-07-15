# Install Jenkins Docker container
docker run -d \
  --name jenkins \
  --network jenkins \
  -p 8080:8080 \
  -p 50000:50000 \
  --user root \
  -v jenkins_home:/var/jenkins_home \
  -v /var/run/docker.sock:/var/run/docker.sock \
  jenkins/jenkins:lts \
  --prefix=/jenkins

- --prefix=/jenkins: to use /jenkins as a base URL

# Update VM nginx to forward the request from internet to the docker container
sudo vi /etc/nginx/sites-available/default
sudo systemctl reload nginx

``` sh
    location /jenkins/ {
        proxy_pass http://localhost:8080/jenkins/;
        proxy_http_version 1.1;
        proxy_set_header Upgrade $http_upgrade;
        proxy_set_header Connection keep-alive;
        proxy_set_header Host $host;
        proxy_cache_bypass $http_upgrade;
    }
```

Forward request to the /jenkins

# Install the Docker CLI to connect to Docker daemon
docker exec -it jenkins bash
apt update
apt install -y docker.io


# Install kubectl
apt update
apt install -y curl
curl -LO https://dl.k8s.io/release/v1.30.1/bin/linux/amd64/kubectl
chmod +x kubectl
mv kubectl /usr/local/bin/

# Install Azure CLI && Connect to AKS cluster
curl -sL https://aka.ms/InstallAzureCLIDeb | bash
az login
az aks get-credentials --resource-group ask-rg --name aks-cluster
kubectl config get-contexts



# Connect the AKS configuration to Jenkins
mkdir -p ~/kube-configs
scp root@192.168.50.10:/etc/kubernetes/admin.conf ~/kube-configs/
ls -l ~/kube-configs/admin.conf
export KUBECONFIG=~/kube-configs/admin.conf