### Definition

1. Docker Basic (knowledge require)

- Image vs Container
- Dockerfile (FROM, RUN, COPY, ENTRYPOINT, CMD)
- Container lifecycle
- Docker registry (Docker Hub / private)
- Basic command PULL, PUSH, RUN

2. Dockerfile – Production level

- Multi-stage build (build → runtime)
- Small images (alpine, distroless)
- Environment variables
- Healthcheck
- Non-root user
- Layer caching (order of COPY/RUN)

3. Microservice-specific Docker

- One service = one container
- Stateless containers
- Externalized config (env, secret)
- Port exposure & networking
- Inter-service communication (HTTP, MQ)

4. Docker Networking

- Bridge network
- Service-to-service via DNS (container name)
- Exposing vs publishing ports
- Local dev vs prod networking

5. Docker Compose (very common in interview)

- Run multiple services locally
- API + DB + MQ
- Depends_on (and its limitation)
- Override configs per environment

6. Security (senior-level topic)

- Secrets not in image
- Scan image vulnerabilities
- Minimal base images
- No credentials baked into image

7. Logging & Debugging

- Stdout/Stderr logging
- docker logs
- Container crash debugging
- Restart policies

8. CI/CD with Docker

- Build image in pipeline
- Tagging strategy (version, commit SHA)
- Push to registry
- Immutable images

9. Docker vs Kubernetes (must explain)

- Docker = packaging & runtime
- Kubernetes = orchestration
- Why Docker knowledge still matters in AKS/EKS
