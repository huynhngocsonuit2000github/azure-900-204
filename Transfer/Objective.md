# 1. Multi-Tenant Architecture

Already discussed — designs for serving many customers (tenants) with:

Shared DB vs per-tenant DB

Isolation, customization, cost trade-offs

# 2. CQRS (Command Query Responsibility Segregation)

Separate read and write paths of your application.

🔹 Writes: Commands → Handlers → Domain Models

🔹 Reads: Direct SQL or optimized view models

✅ Improves performance, scalability, and structure

✅ Often paired with Event Sourcing

# 3. Event-Driven Architecture

Use events to connect services instead of direct calls.

🔹 Tools: Azure Event Grid, Service Bus, Event Hubs

🔹 Promotes loose coupling

🔹 Enables async workflows, retry, failure tolerance

✅ Great for microservices and large-scale systems

# 4. Domain-Driven Design (DDD)

Break your app into meaningful “domains” with their own logic.

🔹 Focus on business language (Ubiquitous Language)

🔹 Use Entities, Value Objects, Aggregates, Services

✅ Keeps code aligned with business rules

✅ Fits clean architecture really well

# 5. Clean Architecture / Hexagonal Architecture

Organize your codebase into layers and keep dependencies flowing inward.

🔹 UI → Application → Domain → Infrastructure

🔹 Core business rules are testable & isolated

✅ Makes codebase scalable, testable, and maintainable

# 6. BFF (Backend for Frontend)

Create an API layer tailored to each frontend (Web, Mobile, etc.)

🔹 Avoid bloated or overly generic APIs

🔹 Helps secure and optimize APIs per platform

✅ Works well with micro frontends or multi-platform apps

# 7. Feature Toggles / Feature Flags

Dynamically turn features on/off per user or tenant.

🔹 Tools: LaunchDarkly, Azure App Configuration

🔹 A/B testing, gradual rollout, and safe deploys

✅ Essential for SaaS flexibility

# 8. Rate Limiting / Throttling

Prevent abuse, overuse, or noisy tenants from affecting others.

🔹 Use API gateways like Azure API Management

🔹 Set per-tenant limits

✅ Improves fairness, stability, and cost control

# 9. Identity Federation / SSO (Single Sign-On)

Allow users to log in using corporate or social accounts.

🔹 Azure AD B2C or Azure AD Multi-Tenant

🔹 Supports SAML, OpenID Connect, OAuth

✅ Required for enterprise SaaS

# 10. Observability Design

Build with monitoring and insights from the start.

🔹 Use correlation IDs, structured logs (Serilog), distributed tracing

🔹 Capture per-tenant usage, errors, performance

✅ Vital for debugging and SaaS SLAs
