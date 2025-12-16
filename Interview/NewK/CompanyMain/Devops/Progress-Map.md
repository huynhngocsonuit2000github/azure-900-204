Drawmap: what you need to learn (EKS + private APIs + AWS services)

``` swift
CI/CD (Bitbucket/Jenkins)
  ├─ build/test .NET
  ├─ build Docker image + push (ECR)
  └─ deploy to EKS (Helm/Kustomize)

EKS/Kubernetes (runtime)
  ├─ Deployments, Services, Ingress/ALB, HPA
  ├─ ConfigMap/Secret, readiness/liveness, resources
  └─ Namespace + RBAC

AWS for private microservices (MOST IMPORTANT)
  ├─ Identity: IRSA (pod → IAM Role via OIDC → STS temp creds)
  ├─ Permission: IAM policies + S3/SQS/KMS resource policies
  ├─ Networking: private subnets, internal LB, VPC endpoints (S3/SQS/STS/Secrets)
  └─ Ops: CloudWatch logs/metrics, CloudTrail audit, alarms

Edge (public only)
  ├─ CloudFront + S3 (Blazor WASM public)
  └─ API entry: API Gateway/ALB (public) → private routing to EKS
```