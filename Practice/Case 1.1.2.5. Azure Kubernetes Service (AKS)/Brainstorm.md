# 1. Basic application

- Create the simple AKS on the Cloud and deploy the simple .Net application and simple FE application on that

1. Create .Net web api with some endpoint
   - Todo Web api app, containerize to container [Done]
   - Run the nginx container to forward request from internet to this .Net container [Done]
   - Connect to Azure SQL server [Done]
2. Create FE app and integrate with this api
   - Simple Angular application [Done]
   - Integrate with those API [Done]
   - Apply simple **LazyLoad** [Future]
3. Deploy to docker and docker-compose
   - Deploy the docker-compose version on this VM, with different sub DNS [Done]
     - <!-- /production/web/ -->
     - <!-- /development/web/ -->
4. Deploy to k8s in local cluster
   - install minikube on this VM [Done]
   - Write the hello world Pod on the minikube, and forward request to sub DNS [Done]
      <!-- http://my-jenkins-mv.westus.cloudapp.azure.com/mini/test -->
   - Write the yaml file to deploy those application [Done]
   - Expose to local browser [Done]
5. Deploy to AKS
   - Create AKS cluster and enable Public IP for Node [Done]
   - Go to **Scale set instance** aks-publictpool-26484877-vmss_0 to update NSG [Done]
     - ICMP for ping command
   - Connect to the AKS API config from local kubectl [Done]
      <!-- az aks get-credentials --resource-group ask-rg --name aks-cluster --overwrite-existing -->
   - Deploy the simple Nginx application on that cluster [Done]
      <!-- http://40.65.61.199:30036/ -->
   - Deploy the application and export to IP [Done]
      <!-- http://52.234.0.105.nip.io/web/ -->
6. Create CI/CD to deploy those simple application
   - Create simple VM to run the Jenkins Agent
   - Create simple job to automate running deploy to AKS
   - Connect AKS config to this VM, to be able to use kubelet

## 2. Enhancement

- Deploy Fakebook to AKS
- CI/CD running the simple jenkins server deploy Fakebook
- Built-in Monitoring & Logs
