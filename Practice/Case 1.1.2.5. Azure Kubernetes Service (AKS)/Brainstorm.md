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
   - Write the yaml file to deploy those application
   - Expose to local browser
5. Deploy to AKS
   - Create AKS cluster
6. Create CI/CD to deploy those simple application
   - Create simple VM to run the Jenkins Agent
   - Create simple job to automate running deploy to AKS
   - Connect AKS config to this VM, to be able to use kubelet

## 2. Enhancement

- Deploy Fakebook to AKS
- CI/CD running the simple jenkins server deploy Fakebook
- Built-in Monitoring & Logs
