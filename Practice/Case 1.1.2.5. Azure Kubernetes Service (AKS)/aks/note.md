# Connect local kubectl CLI to the Azure Kubernets Service (AKS)
az aks get-credentials --resource-group ask-rg --name aks-cluster
    <!-- Merged "aks-cluster" as current context in /home/useradmin/.kube/config -->
kubectl config get-contexts
    <!-- CURRENT   NAME          CLUSTER       AUTHINFO                         NAMESPACE
    *         aks-cluster   aks-cluster   clusterUser_ask-rg_aks-cluster   
            minikube      minikube      minikube                         default -->

# 