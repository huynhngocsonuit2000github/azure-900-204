- Introduce
- Project (why use microservice: cost, scale, independent)
  - Chose Microservice because:
    - this is the big project
    - need the system independent scaling (order, payment, notification), different load traffic, kind of work load
    - separate them let us easy to scale and deploy without impacting whole system.
    - just need to scale necessary service instead of all -> save the cost
    - one service failed will not impact to entire system
- HTTP vs GRPC
  - HTTP: for rest api calling
  - GRPC: It’s faster due to binary protocol (Protobuf) and supports streaming.
- BFF
- azure gateway
- Azure Entra ID
- ensure data consistent
- update data in two services
- Saga pattern: predefine commit transaction + compensation, async commit, eventual consistent
- DP
- SOLID
- DI
- Service Boundary
- data warehousing, power BI, ETL in general
- versioning
- CloudService:

  - S3: static assets
  - SQS:
    - Fully managed (AWS)
    - Built-in retry & DLQ
    - One message → one consumer
  - RabbitMQ:
    - Self-managed (docker instant)
    - Supports complex workflows
    - Best for event-driven microservices

  # SQS → simple, scalable, low-ops

  # RabbitMQ → complex routing, business events, saga flows

- Manage secret information + environment
  - AWS: AWS Secrets Manager, AWS AppConfig
  - Azure: Azure Key Vault
- Log, which data
  - [TraceId], service name, time, userid, retry count
- performance sql
- data integrity: constrain, rule
- trigger and store,
- React & Angular
- S3 access from application only
- working style, leader think

# ==================

- introduce
- solid
- redis
- DI
- s3: VPC, package policy
- resilience
- soft skill

- Service Bus & RabbitMQ
- Dotnet Framework & Net core
- Dotnet in Cloud, AKS support in the same, Code dotnet nhanh hơn, also support GRPC
- EF core optimicsitic
- Data migration (code first)
- architecture
- Sidecar
- Azure pipeline
-

- IAM -> user role -> create policy into user
- assign user to cloudwatch
- Apply VPC

---

- User pool: username, email, usergroup, role, policy
- Lưu thong tin gi trong cognito
- secret manager
- just SQS
- lambda: long running process -> but we need to optimize code
- How to scale Scale:
- multiple RDS, how to handle
- when use IEnumerable
- Singalton + static class
- diamond problem
- migrate data with constraint

# ------ today

- Some service that you working on
- Authentication and authorization in the application
- secret manager, rotation enable
- how app to get the secret
