# Day 1 (C#/.NET) — the important things to learn (6-year level)

## 1. async/await pitfalls, cancellation, HttpClientFactory

- The handle spend lot of time doing I/O (DB, HTTP, MQ), Async key word will let the Thread return to the pool while waiting, in that case it can handle more request
- .Result, .Wait(): it can block the thread, can deadlock, reduce the performance . Use await keyword to fix
- CancellationToken: when the request is cancelled, the API should stop DB/HTTP work
- Task.WhenAll: good way to call multiple independence tasks a time, but it can lead overload the resource (DB - it can DDOS your DB/other service)
  -> Limit the concurrency (SemaphoreSlim) or batch to reduce overhead to the resource (DB/Service)
- “Fire-and-forget”: which mean start the async task but do not await it, the solution for the task we do not want to handle immediately, we can put the work into a queue (message queue) or BackgroundService that you control/retry

- CancellationToken: If the request is cancelled (client disconnect, gateway timeout) we should stop downstream work
- The correct pattern: the controller accept the CancellationToken, pass this variable into the DB/HTTP calls, also set the timeouts ()

### Question

- Why is .Result / .Wait () bad?

  - It block the thread while waiting for I/O (HTTP call, DB request), it will reduce the performance when there are a lot of requests come to the server.
  - In some case it can deadlock
  - In the worst case, the thread pool can be not enough thread to handle the task from client.

- When do you use CancellationToken?

  - I take the CancellationToken in the controller and use it to downstream calls (EF Core, HttpClient, MQ), if the request is cancelled or time out, we stop the work quickly to protect resource
  - // When the request is cancelled, the HTTP connection is aborted, the the ASP.NET core convert that into the HttpContext.RequestAborted (a CancellationToken)

- Is CancellationToken enough to prevent hanging calls?

  - Not always. Because the CancellationToken just be triggered when the client disconnect or you manually cancel.
  - If the dependency is just slow, the call can still sit there if you not set the timeout

- Why use IHttpClientFactory instead of new HttpClient()?

  - Create HttpClient for each request can cause "socket exhaustion" due to short-lived connection lifecycle issue. IHttpClientFactory manage handlers, centralize the configuration (URL, timeout, headers) clean code.
  - // When there are so many connection is opened and closed, we can face the error timeouts or connection refused, IHttpClientFactory can help the connection reusable

- How do you call 3 downstream services in parallel safely?
  - await Task.WhenAll(taskA, taskB, taskC);
  - If one task take time, the whole WhenAll statement can wait for a long time
  - If one task failed, the whole WhenAll statement can throw the exception
    -> In that case using the Task.WhenAll, I add per-call the timeouts and pass the request CancellationToken, and apply the catch for each call failures.

### Another question

1. “What’s the difference between concurrency and parallelism?”
   - Concurrency: handle saving data for one data table at a time, SemaphoreSlim
   - Parallelism: handle multiple independence tasks at a time

2) “When would you use Task.Run in ASP.NET Core?”
   - It move the work to the thread pool ....
3) “What’s wrong with async void?”

   - can not be awaited, in the API server, it can crash the process or get lost, just use for the event handler in the UI

4) “If you await Task.WhenAll, what happens when one task fails?”

   - WhenAll throw exception, we can implement the handle failure for each call if the partial response is acceptable

5) “How do you implement timeout properly?”

   - Set the different time out for each request, because different time consuming

6) “Retry: when is it good, when is it dangerous?”

7) “What is socket exhaustion in simple words?”
8) “Should you use one global singleton HttpClient instead of IHttpClientFactory?”
9) “How do you propagate CancellationToken correctly?”
   - Accept it from the Controller, and pass it down to every async call (EF Core, HttpClient, MQ Publish)
10) “Client disconnected: what do you do?”
11) “How do you avoid ‘thundering herd’ when many requests call the same dependency?”
    - Use Caching where appropriate, and keep the timeout small, limit concurrency
12) “How would you call 10 downstream services? Would you still use WhenAll?”
    - WhenAll just suitable for handling small of number of request at a time, in the case of so many request downstream service, we need to implement the concurrency and batch
13) “What do you log for outbound HTTP calls?”
    - CorrelationId/TraceId, dependency name, URL (without sensitive data), status code, latency, retry count, and failure reason. No secrets or tokens.
14) “How do you handle partial failure in an aggregation API?”

## DI lifetimes, options pattern, middleware pipeline

- DI lifetimes:
  - Singleton: only one instance will be created for whole app lifetime
  - Scoped: 1 instance per HTTP request
  - Transient: new instance for every time it is requested
-

### Question

- Why is DbContext scoped?
  - The DBContext is not Thread-safe and it track changes; The scope life time can prevent the cross-request state leaks
- When do you use Singleton?

  - Use for the stateless services or shared caches/ config provider

- Why not read IConfiguration everywhere?

  - Centralize binding/validation, it easier to test and maintain

- What middleware is?

  - The is handler that every request pass through
    - Exception handling (early)
    - Routing
    - AuthN
    - AuthZ
    - Endpoints

- Where do you handle exceptions globally?

  - I often place the error handling middleware at the first of the pipeline, to ensure we return the consistent error response

- Difference between authentication and authorization?

### Another question

- What are the 3 DI lifetimes and when do you use each?
- Why is DbContext typically Scoped?
  - No thread-safe, track changes, avoid cross-request state leak and concurrency issue
- What happens if a Singleton depends on a Scoped service?
  - Lifetime mismatch, it can throw the error at the runtime or cause the incorrect behavior
- How do you use Scoped services inside a BackgroundService?
  - Create the scope using IServiceScopeFactory
- Why use Options pattern instead of injecting IConfiguration everywhere?

  - Strongly typed setting, central binding, supports validation

- How do you validate config at startup?

  - I add the validation rules and fail fast if required settings are missing/invalid, so we do need to discover it in the production

- Where should secrets live? In appsettings.json?
  - K8s Secrets, AWS Secrets Manager, Azure Key Vault

## OOP, SOLID, LINQ

- OOP, there are four characteristic of OOP

  - Encapsulation: hide the state, expose the behavior
  - Abstraction: expose interface, hide the implementation detail
  - Inheritance: the ability for the class can inherit the proper and method from another class
  - Polymorphism: same interface, different implementation.
    - Parent class Animal have the abstract Move() method
    - Child class Cat inherit from Animal class and implement the Move() method (4 legs)
    - Another child class fish inherit from Animal and implement the Move() method (swim)

- SOLID

  - S - Single Responsibility: one reason to change
  - O - Open/Closed: extend the new code, not modify the old code
  - L - Liskov: derived types must behave like the base type
  - I - Interface Segregation: depend on abstraction, inject implementation
    -> Make the code clean and easier to main, test and change. I often split big service into small services + inject interface

- LINQ
  - Deferred execution (IEnumerable) vs immediate (ToList)

```cs
// # 1
var q = numbers.Where(x => x > 5);

var a = q.Count();   // runs filtering
var b = q.ToList();  // runs filtering again

// # 2
var list = new List<int> { 1, 2, 3 };
var q = list.Where(x => x > 1);

list.Add(10);        // change source after defining query

var result = q.ToList(); // now includes 10

```

- IQueryable vs IEnumerable (EF translation)

### Question

- LINQ: difference between IEnumerable and IQueryable?
  “IEnumerable runs in memory; IQueryable builds an expression to translate (e.g., to SQL in EF).”

  - IQueryable = query is built as an expression tree so EF can translate to SQL
  - IEnumerable = runs in memory using C# code (not SQL)

- When does an IQueryable actually hit the database?
  - ToList/ToArray, First/Single, Count/Any, foreach
  - Before that, it is just the expression tree
- What is “multiple enumeration” and why is it a problem?
  - Deferred queries can re-run each time you iterate them. Use ToList to reuse the result
- Tracking vs No-Tracking in EF — when do you use AsNoTracking()?
  - Use when read-only queries to reduce memory and speed up, just tracking when planing to update entity
- How do you avoid the N+1 query problem in EF?
  - Avoid N+1 by not querying inside the loops
  - Use Include for eager loading

## Background services, resiliency (timeouts/retries)

- Background service (HostedService / BackgroundService): is the code that runs outside the HTTP request, start by the host when the app start

  - Run by itself
  - Must support graceful shutdown (stop token)
  - Access to DB/repository/Service from DI, we need to create scope when we want

- Resiliency: timeouts + retries
  - Timeouts: always set timeout for outbound call (HTTP, DB, MQ publish if applicable)
  - Retry: do not retry blindly, need to waiting for a short time before retry to call next one, to reduce the pressure and avoid retrying immediately

### Question

- “How do you run background jobs in .NET?”

  - Use BackgroundService/IHostedService, when I need scoped services I create the scope using IServiceScopeFactory
  - IHostedService: interface, we will need to implement the StartAsync and StopAsync within the CancellationToken. In that method we can also implement the Timer/long-running Task, Listener
  - BackgroundService: is the base class, we just need to implement the ExecuteAsync method within the CancellationToken

- Why injecting DbContext into BackgroundService is a mistake (lifetime mismatch)

  - Because the BackgroundService is Singleton (one instance for app lifetime)
  - DBContext is the scope (per HTTP request / per unit of work), it is not thread-safe
  - The correct pattern is that we will need to create the scope of this object each time we work

- “How do you stop background work safely during deploy?”

  - using AKS to deploy new version, the container can get "stop" signal, and the running work inside the BackgroundService will be stop immediately
  - So we need to implement the StoppingToken(CancellationToken)

- “What’s the risk of retries?”

  - It happen in the case you call service to update something, but the network timeout or some unexpected issue, and nodes not return the response to the caller, and the retry logic can perform the step calling again.
  - To avoid that issue
    - Implement within unique constraint
    - Implement Outbox/Saga pattern

- MQ publish issue + retry + Outbox (simple but deep)

  - Scenario: Order exists (in the first DB), but publish event never sent (because of network issue) → other services never react → system inconsistent.
  - Solution: using Outbox pattern (reliable publishing). When we insert the order, we will need to insert the outbox row (event payload). Then the background worker read pending outbox row and publish to MQ, make it as sent
    -> Publishing to MQ can fail after DB commit, causing lost events. Retries help only for short outages. Outbox solves this by storing events in the same DB transaction, then a background worker reliably publishes with retries. Because duplicates can happen, consumers must be idempotent.

- The order of the Pipeline
  - ForwardedHeaders (reverse proxy / ingress / Nginx)
  - Exception handling (global)
  - CorrelationId + Request logging (very early)
  - HTTPS redirection + HSTS (if you own TLS at the app)
  - Routing
  - CORS
  - Authentication
  - Authorization
  - Rate limiting (if used)
  - Endpoints
