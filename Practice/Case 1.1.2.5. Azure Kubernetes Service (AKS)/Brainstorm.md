# 1. Basic application

- Create the simple AKS on the Cloud and deploy the simple .Net application and simple FE application on that
  
1. Create .Net web api with some endpoint
    - Todo Web api app
    - Connect to Azure SQL server
2. Create FE app and integrate with this api
   - Simple Angular application
   - Integrate with those API
3. Deploy to docker and docker-compose
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