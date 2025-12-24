1. Son -> Toan -> Duc \*
2. Toan -> Duc -> Son
3. Duc -> Son -> Toan

========

0. Additional

- Span<T>
- Garbage collection
- Task & ValueTask
- Filters middle
- API versioning
- Minimal APIs (Healthcheck)
- HTTP status code
- SQL injection, XSS
- Zero-downtime database migrations
- IHttpClientFactory === Connection Pool === Thread Pool
- Distributed Caching, idempotent (in microservice)
- retries, timeouts, and circuit breakers?
- SOLID, Clean code

======

1. IEnumerable & IQueryable

2. async/await, common mistake

- HttpReuqest : Main Thread
- Wait for Task .... without blocking main thread
- common mistake:
  - Task.Result / .Wait() => block main thread

3. DI

- Loose Coupling
- Interface + Implementation + Inject runtime

4. Singleton, Scoped, Transient

5. Middleware & Pipeline

- Middleware: [component] that request will go through (globalexception, authen, author......)
- Pipeline: define the [order] of middleware

6. RestFul + HTTP status code

- [Standard] + [Action-Method] /get/post/put...
  /ORDER
- Example status

7. validation

- Only validate at model
- Custom attribute

8. Authenticate & Authorization & Entra

- Flow + Cookies (BFF)
- Authe: check access to app (BFF)
- Author: check role & permission (Gateway)

9. JWT

10. Secret Information

- appsetting.json
- Encript

- Key vault, Environment

11. N + 1 problem

- 1 get list parent
- next n -> children
  var parent = dbContext.Set<Parent>()

```
foreach(var item in parent)
{
    var child = item.Children; // call db -> lazy loading
}
```

=> dbContext.Set<Parent>().Include() // Eager loading

12. AsNoTracking

- [Not-Track-Entiry-State]

13. Transaction & Any design pattern

14. API performance

- Monitor -> rootcause
- Check code, optimize code

- Filter before Join
- Check exe plan
- Create index

15. connection pooling

16. Caching

- Memory

17. long-running background jobs

- Improvement user experience (not block main thread)
- Proceed at the time that little user access
- Separate application, Azure Func, Cloud service

18.
