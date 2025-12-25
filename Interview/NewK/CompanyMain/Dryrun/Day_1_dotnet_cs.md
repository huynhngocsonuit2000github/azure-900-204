1. DI + Lifetime
2. IQueryable + IEnumerable
3. Design pattern

- Monolith

  - Repo sitory and Unitofwork:
  - Factory: (Report name) -> corresponding Report SQL -> Use the same Core function
  - Adapter: example
  - DI:
  - Autofact:

- ## Microservice

  [TODO]

4. Stack & Heap memory

- Prod stack: faster Heap
- Cons of stack
  - Easy lead to out of memory if not handled properly
  - HARD to work on multiple threads

5. Middleware and Pipeline

- M -> function (logging / auth / exception handling)
- (QUEUE) Set of M, define sequency, Order is important

6. Global exception

- Format, reduce unnecessary information (stack trace)

7. IHttpClientFactory

- Exhauted resource
- Pool of HttpClient object

8. Calling downstream system - failed issue

- Use retry + small timeout, set number of timeout failed

9. N + 1 problems

- Eager loading: Include()

10. AsNoTracking?

11. Auth

- Entra + SSO

12. Secret

- Mono: Appseting + Key encrypt
- Micro: no appsetting + environment variable

13. Caching

- Memory
- Distributed (redis)

14. All of the ThreadPool are unavailable

- Load test (503 service unavailable, bad gateway, connect refuse)
  - Bad code
  - Long process hold thread
  <!-- - Deadlock (thread) -->

15. BackgroundService

- Long running Process
- Scheduler

====================

# Advanced .NET API interview questions (3–6 years)

## Architecture / Design

How do you design API versioning (URL vs header) and keep backward compatibility?

How do you do zero-downtime DB migrations in production?

When to use CQRS vs a normal CRUD API?

How do you handle cross-service queries (API composition vs read model)?

How do you design idempotency for POST/PUT in real systems?

## Reliability / Distributed systems

Explain timeouts, retries, circuit breaker, bulkhead — what order and why?

How do you avoid duplicate message processing (idempotent handler + inbox)?

What is the outbox pattern and why it matters?

How do you guarantee event ordering and what if events arrive out of order?

How do you handle eventual consistency and explain it to product/QA?

## Performance / Scalability

How do you identify bottlenecks: CPU vs IO vs DB?

How do you tune SQL queries + indexes from API symptoms (slow endpoint)?

Why can async/await still be slow? Common pitfalls?

How do you handle large file upload/download without loading into memory?

How do you prevent thundering herd on cache miss?

## Security

How do you secure service-to-service calls (client credentials / mTLS / gateway)?

How do you implement fine-grained authorization (policy-based, resource-based)?

What is the risk of JWT long lifetime? How to do refresh / revocation strategy?

How do you avoid leaking data in logs (PII) while keeping observability?

## Observability / Operations

What do you log for production debugging? (traceId, spanId, userId, etc.)

How do you implement distributed tracing (OpenTelemetry) end-to-end?

What metrics would you alert on for an API? (p95, error rate, saturation)

How do you do health checks (liveness/readiness) with dependencies?

## .NET internals / tricky

Explain GC pressure and how it shows up in API latency.

What issues come from using Singleton with mutable state?

How do you safely run background work (IHostedService) without breaking request scope?

Explain EF Core concurrency (optimistic concurrency token) with a real example.

=======
1.25

8 CTMS
tự học + ko có tiền + mối trường -> thoải mái

10 2 proejct (phức tạp)
môi truong + tiền (tiền học) + thực tế ->
