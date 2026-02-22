Please generate for me 5 interview question in microservice regarding topic: configuration/secrets strategy

# Knowledge

service boundaries, DB-per-service, API composition
sync vs async communication, eventual consistency
versioning
delivery semantics (at-least-once reality), dedup/idempotency
retries, backoff, DLQ, poison messages

<!-- ordering guarantees and how they break -->

why 2PC is avoided; consistency patterns
Outbox: transactional event publishing, dispatcher, retries
Saga: choreography vs orchestration, compensation

# Practice:

explain failure scenarios + how Outbox/Saga handles them

Microservices operational concerns
• configuration/secrets strategy => secret
• backward/forward compatibility for events
• observability basics: logs/metrics/traces, golden signals

# Practice:

• debugging story: “message duplicated”, “consumer lag”, “slow DB”

===============

- service boundaries: design microservice

  - Logic, feature
  - One service have to have one DB
  - Not access to another DB directly (API)
  - Communication: avoid sync API call, have to use async call (Message Queue)
  - Avoid distributed transaction: create multiple record on multiple services
  - Independent deployment

- DB-per-service:

  - No sharing DB for multiple service
  - If share => break loosely couple / Independent
  - Avoid impact scope

- API composition / BFF (backend for frontend)
  <!-- 10 services, UI integrate 10 service -->

  - CORS
  - Reduce network load
  - Sharp data (=> model) => return UI
  - UI call /bff/home
    - BFF call 5 API services (Sync, HttpClient)
    - Return UI 1 response
  - Partial failure, fail fast
  - Cache
  - Authenticate Azure entra ID (cookies)

- sync vs async communication

  - Sync: HttpClient - call api
    - Get data immediately
    - No create record (distributed transaction) => because system performance, user experience
  - Async: message queue
    - Create (later)
    - achieve eventual consistency

- Trade off eventual consistency <> good system performance

  - accept late update -> not a bug

- versioning (new feature + DB schema change)

  - mark URL within version /api/v1/users
  - Create new column -> implement feature (not delete old column)
  - deploy: rolling deploy
    - old and new version
    - all version is new
  - Remove old column -> remove old implementation
  - deploy
    - all the code is new

- Outbox / Inbox: delivery semantics (at-least-once reality)

  - at least 1 message should go to the queue
  - Case 1 (no outbox)
    - Create record -> save db
    - Publish to MQ (even we have retry)
    - If MQ down -> lost data
  - Case 2 (outbox)
    - Create record + create event record (OutboxTable) => save change
    - Scheduler: scan pending event => publish MQ
  - Inbox: create EventId as unique ID

- idempotency / dedup:

  - avoid duplicated work
  - Table (EventId)

- retries, backoff, DLQ, poison messages

  - Retry: always apply for temporary error => resolve temporary error
  - backoff: wait time will be increased for the next retry => avoid over load to the server
  - Number of failure:
  - DLQ: Queue - contain poison messages
    - Not able to work (not related to business)
    - Notify
    - Dev will check
    - Fix code/ fix data

- why 2PC (Two-Phase Commit) is avoided; consistency patterns

  - Block / Break distributed system
  - Does not scale
  - Instead, we use local transaction -> achieve eventual consistency
    - Outbox, idempotency, Sage, compensation

-
