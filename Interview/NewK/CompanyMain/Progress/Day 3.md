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
Idempotency → prevents duplicates (event Consumer and Http)
Saga → fixes business failures (some case need to handle Success/Failed/Time-out/ Reject)
DLQ → exposes poison messages (implement the compensation action on the Saga pattern)
```

- WHY THIS DESIGN (INTERVIEW GOLD)

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

## Logger

- In the local development, I use Serilog + central log store (we run the docker to start the log server) this is the central log server. And the I config the Log to point to that server, then every log will be stored on that server
  -> Locally, I use the Serilog with the central log store (log server), all the service logs and Seq, structure with IDs, Thread Id, Process Id, the message, the time, the message level. So I can trace async workflow end to end (by using the Correlation Id). In the production, the logs go to Azure Monitor or ELK

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

- What is the CorrelationId?

  - Unique Id created at the first entry point of request, and pass this Id across all service and async message to link logs and traces for one business action flow. If we filter by Correlation Id, we will see the full business action flow

        - Which service failed?

        - I filter the logs by Correlation Id and look for the Error or missing event to identify the failing service. And I also detect it in the DLQ

        - Did retry happen?

        - I check the retry count, repeated logs with the same MessageId under the same Correlation Id

        - Did idempotency skip?
        - Look into the log to see any message show that the DuplicateMessageSkipped or not

    -> First I identify the Correlation Id from the entry API. Then I filter the centralized logs by that ID. I look at the error logs or missing step to see which service failed. If the same message with the same MessageId multiple times, I know that retry happen or there is an error with publisher that delivery the same message many times. If the log with the message like DuplicatedMessageSkipped, which mean Idempotency handled duplicates correctly
 
- How to dev the big project with multiple services? 
  - Just run the necessary service we are working on, do not run all of the service
  - There is a share dev environment, there are running all of the service for entire system
  - And the local running service will connect to the share dev service to be able to full flow integration

### Another question
- How do you know a service boundary is wrong?
  - The service are depend to each other
  - The update for one function need the change from many service
  - Many sync calls to multiple service api for one business action
- Can two services share the same database?
  - In microservice we should not share the same database for two service
  - It will be tight couples deployment, schema, and runtime. One new or improvement for one service can impact to other service, and need the deployment together
- How do you handle queries that need data from multiple services?
  - We can use API composition
  - BFF in the case of Web UI call to microservice, UI will use BFF to avoid so many call to microservice api, then it just call to BFF only one time
  - CQRS read model
- How to dev the big project with multiple services?
  - Run necessary service on the local side to develop the feature
  - The local service will connect to the shared service dev environment
  - There is a development environment, which will run all the service. This support for developer can work to debug
- What if OrderPlaced is published but Payments never sees it?
  - There are some reason the Order publish the message but it is not available on the queue, so the Payment service can not see that message to consume, so the business action can not be perform full flow (network, message queue down for a short, MQ crash)
  - Apply the Outbox and retry
- What if Payments processes OrderPlaced twice?

  - Use Idempotency pattern in the Payment, in which, when we send the message, we will need to send the unique ID together, and in the consumer service, we will check if the ID already handle or not, if handled we will ignore

- What if Orders receives PaymentAuthorized twice or out of order?

  - To avoid handle message two times, we will implement Idempotency pattern

- What if payment succeeds but Orders update fails?

  - Order will retry to consume the event

- How do you handle cancel when payment is already authorized?

  - Implement the compensation

- Saga vs orchestration here?

  - There is some special business action, we need to implement the saga pattern, in which we will define all the necessary scenario for one business action can have, for example when Order is Place(Inventory will check), Order is Confirmed(Inventory reduce the production), Order is Cancel(Inventory rollback)

### Another question

- Service boundaries: How do you decide boundaries beyond “one DB per service”?

  - One service = one business job: will perform clear business action or functionality
  - One service owns it data: this allow to change update data, other service will read the data via API or event only
  - If the feature/functionality change together, they should stay together
  - If the data must be correct immediately -> same service
  - If the data can be correct independently -> we will separate into different service, and communicate via MQ
  - So many sync calls between services -> Wrong boundary
  - Need distributed transaction

- DB-per-service vs shared DB (real tradeoff): When is shared DB acceptable?

  - In the migration phase

- Sync vs async decision: When do you require synchronous calls between services?
  - For the synchronous: When the current functionality in the service need immediate data from another service (Ex: order function need the pricing detail, validate some information)
  - For the asynchronous: we the functionality is the long process need more time to handle, and need to handle across many different service. We can also apply for the business action it can happen many different case or result. In that case it will not block the user experience
- API composition: BFF vs API Gateway vs “Aggregator service”?

  - Gateway: is use to forward the request to corresponding API service, in the Gateway, we can apply routing, authentication, authorization, rate limit
  - BFF: is behavior like Gateway, but is often use to support for specific UI client (Ex: Blazor WASM call to only /bff endpoint, BFF will call to 5 API service to get the data and return to the WASM)
  - For multi-service UI reads, prefer BFF + query model.

- CQRS read model: How do you keep read model fresh and correct?

  - In some case, using BFF can also not resolve the problem call to multiple service API to get the data immediately, if on service failed or slow it can impact to the entire service performance. So we can also apply CQRS pattern, we will create one CQRS service, this is used to prepare the data to respond to the UI, and the read-only database to combine the data from multiple service. So whe the UI want to get the data from multiple service, it just need to call to that CQRS to get the data
  - Build the consumer that listener to the Message queue only, and implement the idempotency pattern to ensure there is not duplicated work, implement the retry also,

- Exactly-once myth: How do you design for at-least-once delivery?

  - I will apply the Outbox pattern/Inbox pattern/ Idempotency pattern, in which:
    - Outbox: when commit the business flow to database, we will save the message item in the same transaction, and there is a scheduler to scan an publish this message to the QUEUE, to ensure at least one message go to the queue
    - Inbox pattern: with each message, we will attach the unique ID
    - Idempotency pattern: in the consumer will check this unique ID already handled we will ignore

- Ordering guarantees: How do you ensure business ordering of events?

  - All the events of the same Business Action Id will go to the same Message Queue (Service Bus, MQ). That will ensure all service will handle the same business action with the correct order of the step
  - We will implement the code to handle the workflow to make all the case happen this is still the expected case

- Distributed transactions: Two-phase commit?

  - Actually, We need to narrow case of two-phase commit
  - Implement the Saga pattern, to prepare all of the necessary case it can happen with the one business action
  - Implement the compensations

- Consistency & invariants: Where do you make sure business rules are not broken?

  - Avoid Distributed transaction
  - Avoid writing directly to Payments DB
  - Rule across service will be handled with event and Sagas pattern using eventual consistency, dot direct database write

- Observability fundamentals: What do you require in production?

  - CorrelationId/traceId propagated end-to-end, structured logs, distributed tracing

- Testing strategy: How do you test a microservices system without brittle end-to-end tests?
  - Unit test for specific function
  - Contract tests for APIs/events
  - component tests with testcontainers
