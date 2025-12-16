# The key idea
Pipeline + Kubernetes only gets the pod running. We still need to use AWS service (S3/SQS/RDS/Secrets Manager...) To do that in production, we need to solve 3 things
- Identity: how the pod gets AWS credential, so it can call S3/SQS/RDS...
- Permission: even with credential, AWS can still say AccessDenined
- Network + Operations: even with both, the pod might still not reach AWS endpoint

# Identity: how the pod "log in" to AWS
