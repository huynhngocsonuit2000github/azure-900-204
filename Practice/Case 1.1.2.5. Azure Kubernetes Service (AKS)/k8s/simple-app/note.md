# Frontend tag
docker tag angular-todo-app huynhngocsonuit2000docker/azure-image:angular-todo-app

# Backend tag
docker tag todo-app huynhngocsonuit2000docker/azure-image:todo-app 

# Push
docker push huynhngocsonuit2000docker/azure-image:todo-app 
docker push huynhngocsonuit2000docker/azure-image:angular-todo-app


# get minikube ip
minikube ip
curl http://10.0.0.4:30500
curl http://10.0.0.4:30080
