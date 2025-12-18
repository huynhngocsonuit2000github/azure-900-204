# Microservices fundamentals

## Service boundaries, DB-per-service, API composition

- Service boundaries:

  - Each service will have specific data and business logic, we can know as Single Responsibility
  - Independent development: for example each team handle one service without any blocker
  - Reduce the impact of the risk of the change the system

- DB-per-service:

  - One service will have one DB, another service can not query on that database
  - Data sharing happen via APIs or events, not SQL
  - Avoid the dependency between the services
  - Reduce the impact risk to each service

- API composition
  - This is the way to combine the data from multiple service without sharing the database.
  - For example, BFF (backend for frontend) or API gateway: this is the way to combine multiple services into only one service
  - It combine the response from multiple API service and return back to UI

### Question

- How do you define service boundaries?
  - Define the service boundaries by Business logic, not from technical layers
  - Each service will have its data and logic
  - So it can change the logic and deploy independently
- Why is shared database a bad practice?
  - If we share the database to multiple service, it will create the dependency between service
  - The schema change in one service can impact to other service
  - But you know, the core concept of the microservice that is independently, but the server depend to each other can break the flow
  - If the database failed, all of the service depend on that database will be down
- How do services share data without DB joins?
  - Share the data through APIs synchronous read, for example, the Order service call to User service to get the data
  - Events for asynchronous updates (Message queue), accepting eventual consistency
- How do you build a UI that needs data from multiple services?
  - The UI will not call to multiple API service directly to get data
  - I will use API composition via BFF or API gateway, in that case, with only one /bff endpoint API we can call to multiple APIs to get data, and return to the UI in only one respond
- What is eventual consistency and why is it acceptable?

  - It means the data is not updated everywhere at the same time for consistently
  - When the user service update the username, then the order service will wait for short time to be updated, there is a short time to show the different values
  - It is acceptable
    - Because the services are independent
    - It is good for scales the instance of the service

- When would you use BFF instead of API Gateway?

  - When UI needs are specific and complex.
  - BFF gives flexibility, for example we have multiple platform like Web app, mobile, admin UI, with each platform, it need the different data model
  - BFF will help the UI to fetch exactly data UI need, and combine multiple API service calling into only one bff endpoint
  - Using BFF to help to reduce the number of API calling from UI to server

- What are signs that service boundaries are wrong?
  - Only one request by need to communicate to many service to handle
  - Multiple service need the same tables -> the schema change from one service can impact to other service
  - Need to deploy many service together for one change
    -> Complexity increased without real benefits
  - If the service depend to each others, need to deploy together, when the service break, it break together -> boundaries are wrong
- Can service boundaries change over time?

  - Yes, the service boundaries can be changed overtime.
  - Because the domain, the business grows day by day, so the demand of the system is also increased and the system also need to refactor rely on the demand.
  - For example slit the service, merge the service, move the responsibilities as expected

- How do you avoid chatty communication between services?

  - Avoid call multiple API to get the data, if possible we can call 1 time to return a full model of data, Use only one API to return everything needed
  - Use async events (publish message to queue) instead of multiple sync calls. No need to way for other service to get the response, reduce the network overhead on other service

- Is DB-per-service always a separate physical database?

  - One service should have one own DB
  - The schema change will be safer
  - Independent with other service, so it is good for scalability
  - If the database is broken, it will just impact to on the one dependent service, will not impact to many service

- How do you handle reporting across multiple services?
  - Do not query service database directly for reporting
  - When the data change from one service, this service will push the event message to queue, and the reporting service will listen to the event, and update the report database. It is good for near-real-time reports, operational dashboards
  - Data warehosue, we can read the data from the data warehouse to generate the report, and the is another job to collect the data from multiple service databases into database ware house
- How do you enforce DB-per-service in a team?

  - Separate database credential for each service
  - No share the database user

- Where should API composition NOT happen?
  - Show not happen in the composition. Because the Domain service (Order, Payment, User), they should not depend on other service data
    - Is just used to validate the business rule
    - Processes command (create order, pay the invoice)
    - Publish event about the changes
    - Expose the APIs about its own data only
  - API composition is just like the middle layer, we often use to combine multiple API calling to respond only one data to UI
- What are the risks of API composition?

  - Higher latency -> need to apply the timeout and retries
  - Partial failure -> fallback, stop calling the failed service
  - We should apply the cache for those BFF endpoint, to avoid the case of use request so many request in a short time

- How do you reduce latency in API composition?

  - Parallel calls, caching, optimize performance for each service, api calling, database query still the best way to improve the performance for entire application

- How do these three concepts work together?

## Sync vs async communication, eventual consistency

- Sync: Http, the service A call to service by using Http and get the response from service B immediately, depend on the API handling. But it can tight couple, cascading failures, latency. Use when we want to get the query data immediately to proceed the next step for function demand, strong consistency needed
  - Use to Query API (GET)
  - Auth / validation / checking something
  - Low-latency read paths
- Async: Message queue / event bus (Kafka, SQS, Service Bus). It will loose couple, scalable. Lead to the case the data inconsistent at the short time (eventual consistency), harder debugging. We use in workflow, integration, high throughput
- Eventual Consistency:
  - The data will be inconsistent for a short time, will be sync but not immediately
  - Example: The service order save the order -> emit the message to Message Queue with event OrderCreated -> Payment and Inventory update later, so the data for Order/Payment/Inventory service will be inconsistent for a shot time
  - Because the system are split into multiple service and database, to achieve immediate consistency data, would slow everything, so the update happen asynchronously
  - Common pattern: Saga, Outbox, Idempotency
- Why this is matter [!]
  - Avoid distributed transactions
  - Improve availability & scalability
  - Async processing scales better under load
  - Temporary failure do not stop the system
  - Benefit:
    - Finish the API fast, improve the user experience
    - The consumer can slow or retry safely, do not need to handle immediately
    - The services can work independently
  - Trade-offs:
    - No immediately result
    - More infra (MQ, DLQ)
    - Debugging needs tracing
- Microservice mindset

  - Inconsistent state, it is not a bug. Because we would like to improve performance for system by accepting the inconsistent data for a short time
  - Systems are built to heal themselves over time [todo]

- [Outbox] Order + Event saved in the same database transaction. Then there is a scheduler will scan the pending event, to sent to Message Queue

- [Saga] If the Payment service fails after retries, then the Payment method emit back to the Message Queue with request PaymentFailed, the Order service will makes order Cancelled, release the inventory -> The system will reach a valid end state

- If the message was retries so many time but it still failed, then we will move this message to the Dead Letter queue, then we can inspect that, and the system can still running

- Some pattern
  - Outbox → no lost messages
    - Problem: The database save successful but the event publish failed
    - Save the Order to database and event OrderCreated in the same Database transaction
    - The background scheduler will scan and publish the pending event
      -> Ensure: Order and event are always consistent
  - Idempotency → safe retries:
    - Problem: There are some case, one message was delivered two times -> Duplicated work
    - Each message will have the Unique ID
    - Then the consumer will ignore the message if that Unique ID already handled
      -> Ensure: retry by the same result, not duplicated work
  - Saga → controlled rollback:
    - Problem: There are some functions which have multiple step to be done, need to be done in multiple service
    - Each step should implement the roll back action (compensation action)
      -> Ensure: the system ends in a valid state
  - DLQ → visibility, not downtime
    - Problem: Message keep failing forever
    - When retry many time failed -> will move the message to Dead Letter Queue
    - The system continues running
    - Engineers:
      - Inspect the message
      - Fix data or code
      - Reply event
    - Ensure there is not issue happen

```
Outbox → ensures event exists
Retry → tries delivery
Idempotency → prevents duplicates
Saga → fixes business failures
DLQ → exposes poison messages
```

<!-- - WHY THIS DESIGN (INTERVIEW GOLD)
  - Async = resilience, performance improve, increase user experience
  - TraceId = Business action (User place an order), Correlation Id, it helps to easy to debug the end to end flow
  - Idempotency = correctness, ensure the same message will be handled only one time, there is no duplicated work
  - Eventual consistency = scalability, fast response to the user

- Common mistakes (red flags)
❌ No correlation ID
❌ Plain text logs
❌ No DLQ
❌ No retry visibility
❌ Logging only in producer, not consumer 

https://chatgpt.com/g/g-p-693ac6dedfc48191b9eecfebb854b00c/c/69418fe8-3c28-8324-a580-e9afabbd9061
-->

### Question

- How to debug within multiple services?
  - Using CorrelationId, one CorrelationId will be corresponding to one business action
    - We often generate the Correlation Id at the API gateway
    - And pass this Correlation Id through entire downstream service, and include that inside the Message
    - Search the log by the correlation id
  - Message metadata will be included
    - Correlation Id (Trace Id)
    - Message Id
    - Service name
    - Event name
    - RetryCount
  - Why
    - To detect the duplicate
    - Track retries
    - Trace business flow
  - If the Message was retried for many times, this message will be moved to DLQ, log within the Correlation Id
  - Idempotency + Logs
    - We can trace the duplicated message detected
    - The system will check the MessageId (Unique Id) to handle the requestion safety, no duplicated work
    - Idempotency: will check if the message was already proceeded or not, if the message was done, it will log the event like Skipped
- The log will be stored in the Azure Monitor, then we can query by the traceId = correlationId.
  - This will help to debug async workflow like sync
  - Rebuild business flow