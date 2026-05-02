# Order Processing Service

A minimal microservice implemented in C# (.NET 10) that processes incoming orders asynchronously and stores them in a PostgreSQL database.

## Features

- **HTTP API**: Submit orders via a RESTful endpoint.
- **Asynchronous Processing**: Uses RabbitMQ and the Outbox pattern for reliable background processing.
- **Persistence**: PostgreSQL for order and inventory data.
- **Observability**: Prometheus metrics and structured logging.
- **Containerized**: Fully Dockerized for easy deployment.

## Prerequisites

- Docker and Docker Compose
- (Optional) .NET 10 SDK if running locally

## How to Run

1. **Clone the repository**:
   ```bash
   git clone https://github.com/ArturBrutjan/orders-service.git
   cd OrdersService
   ```

2. **Start the services**:
   ```bash
   docker compose up -d
   ```
   This will start the application, PostgreSQL, RabbitMQ, and Prometheus.

3. **Access the API**:
   - The API is available at `http://localhost:9999/swagger`.
   - Use the `POST /orders` endpoint to submit an order.

4. **Example Request**:
   ```json
   {
     "customerId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
     "items": [
       {
         "productId": "00000000-0000-0000-0000-000000000001",
         "quantity": 2
       }
     ]
   }
   ```
   *Note: Seed data includes product IDs `00000000-0000-0000-0000-000000000001` to `...003`.*

5. **Check Metrics**:
   - Prometheus is available at `http://localhost:9090`.
   - Look for the `orders_processed_total` metric.

## Design Decisions & Trade-offs

### 1. Asynchronous Processing with RabbitMQ
- **Decision**: Used RabbitMQ as the message broker.
- **Reasoning**: RabbitMQ is a mature, reliable, and widely-used message broker that provides strong delivery guarantees. It's better suited for task queuing compared to Redis (which can lose data if not configured carefully) for this use case.
- **Trade-off**: Slightly higher infrastructure overhead compared to simple in-memory queues or Redis.

### 2. Outbox Pattern
- **Decision**: Implemented the Outbox pattern for message publishing.
- **Reasoning**: To ensure atomicity between database updates and message publishing. The order is saved and the event is stored in an `Outbox` table in the same transaction. A background worker (`RabbitMqProducerService`) then polls the table and publishes messages to RabbitMQ.
- **Trade-off**: Increases complexity and introduces a small latency between the API response and the message reaching the broker. However, it prevents "ghost" messages or lost events if the broker is down.

### 3. PostgreSQL for Persistence
- **Decision**: Used PostgreSQL.
- **Reasoning**: Standard relational database with strong ACID compliance, perfect for order processing where data integrity is paramount.
- **Trade-off**: Requires more configuration than NoSQL for schema changes, but worth it for the reliability.

### 4. Background Worker Implementation
- **Decision**: Separate producer and consumer services running as `BackgroundService` within the application.
- **Reasoning**: For a minimal example, keeping them in the same process simplifies deployment while still demonstrating the asynchronous flow. In a real-world scenario, the consumer might be scaled independently.

## Assumptions

- **Inventory Seeding**: The database is automatically seeded with a few inventory items on startup to allow immediate testing.
- **Validation**: Basic inventory check is performed at the API level (synchronously), while enrichment (price calculation) is done in the background.
- **Idempotency**: In this minimal version, strict idempotency checks for event processing are not implemented but would be required for production.
- **Security**: No authentication or authorization is implemented for this demonstration.

## Observability

- **Metrics**: Uses `System.Diagnostics.Metrics` to export `orders.processed` counter.
- **Logs**: Structured logging using the default .NET logger, visible in Docker logs.
- **Health Checks**: Docker Compose includes health checks for RabbitMQ and PostgreSQL to ensure correct startup order.

## Out of Scope (Future Improvements)

While this service provides a functional baseline, several production-grade features were intentionally omitted for simplicity:

- **Proper Migration Handling**: Currently, migrations are applied on startup. In production, this should be handled as a separate step (e.g., init containers or CI/CD pipelines) to avoid race conditions during scaling.
- **Retries & Dead Letter Queues (DLQ)**: The consumer currently ignores failed messages. A robust implementation would include exponential backoff retries and a DLQ to capture and investigate failed events.
- **Application Health Checks**: While infrastructure has health checks, the application itself lacks a `/health` endpoint to report its internal state to orchestrators like Kubernetes.
- **Strict Idempotency**: The consumer does not track processed message IDs, making it susceptible to duplicate processing if RabbitMQ redelivers a message.
- **Distributed Tracing**: Adding OpenTelemetry for cross-service tracing would be essential for debugging asynchronous flows in a larger system.
