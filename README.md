# shop-demo — example microservices storefront

This repository is a full-stack demo showcasing a clean microservices architecture for a small storefront. It includes:

- ASP.NET Core (.NET 8) microservices for product and order domains
- An Ocelot API Gateway that routes and enforces policies
- An Angular 20 frontend (with optional SSR) served by Nginx in production builds
- Docker support and a `docker-compose.yml` to bring the system up for local integration

Goals: demonstrate separation of concerns, routing via an API gateway, and a simple containerized developer experience.

## Repository layout

- `api/` — backend projects (API, service, domain, infrastructure)
  - `example.order.api/` — Order API
  - `example.product.api/` — Product API
- `gateway/` — Ocelot API Gateway
- `fe/` — Angular frontend application

## Architecture overview

The system follows a simple API gateway pattern:

- The frontend issues HTTP requests to the API gateway.
- The gateway routes requests to the appropriate downstream API (orders or products).
- Backend services are independent ASP.NET Core apps organized by domain and can call into additional services or databases as needed.

Data-flow diagram (Mermaid)

```mermaid
flowchart LR
  subgraph FE[Frontend]
    A[User Browser / SPA]
  end

  subgraph GW[API Gateway]
    B[Ocelot Gateway]\n(Host: container 'gateway', host port 5000)
  end

  subgraph BE[Backend Services]
  C[Order API]\n(service: order-api, container port 80, host 5001)
    D[Product API]\n(service: product-api, container port 80, host 5002)
  end

  A -->|HTTP (REST/JSON)| B
  B -->|/api/orders/*| C
  B -->|/api/products/*| D
  C -->|DB / other services| E[(Database / external services)]
  D -->|DB / other services| E

  classDef infra fill:#f9f,stroke:#333,stroke-width:1px;
  class E infra;
```

If your Markdown renderer supports Mermaid the above will render as a proper diagram. If not, see the ASCII fallback below.

ASCII fallback (FE -> Gateway -> Services)

Frontend (browser)
  |
  | HTTP request -> Gateway (host:5000)
  v
Gateway (Ocelot)
  |---> Order API (order-api:80)  [host mapped 5001]
  |---> Product API (product-api:80) [host mapped 5002]

## Quick start (Docker / docker-compose)

This repo includes `docker-compose.yml` at the project root which builds and runs the frontend, gateway, order-api and product-api on a single Docker network.

From the repository root:

```powershell
docker-compose up --build
```

Open in your browser:

- Frontend: http://localhost:4200 (Nginx serves the production bundle)
- Gateway: http://localhost:5000 (forwarding to backend services)
 - Order API (direct, if needed): http://localhost:5001
- Product API (direct, if needed): http://localhost:5002

## Architecture diagram (SVG)

Below is a rendered architecture diagram that shows the main data flow from the frontend through the gateway to the backend services and the database/external systems. If your viewer doesn't render images inline you can open `docs/architecture.svg` directly.

![Architecture diagram](./docs/architecture.svg)

Caption: Frontend -> Ocelot API Gateway -> Order/Product APIs -> Database / external services. Host port mapping shown in the legend of the diagram.

Notes:

- The gateway (`ocelot.json`) is configured to route to the Docker service names `order-api` and `product-api` on port 80 so internal container routing works correctly.
- For local development you can run the frontend with `npm start` (ng serve) instead of the container — consider adding `docker-compose.override.yml` to make this convenient.

## Local development (PowerShell, without Docker)

- Backend APIs (example):

```powershell
cd .\api\example.order.api\example.order.api
dotnet run

cd .\api\example.product.api\example.product.api
dotnet run
```

- Run the gateway:

```powershell
cd .\gateway\example.api.gateway
dotnet run
```

- Run frontend in dev (hot reload):

```powershell
cd .\fe\example
npm install
npm start
```

If you run locally without Docker, update `gateway/example.api.gateway/ocelot.json` or the project launch profiles so the gateway routes to the correct host/ports (for example `localhost:5001` for orders).

## Configuration and environment

- Gateway routing: `gateway/example.api.gateway/ocelot.json` (routes and rate limits configured there)
- App settings: each API project has `appsettings.json` and optionally `appsettings.Development.json`
- Docker: each service contains a Dockerfile; the compose file in the repo root builds images from those Dockerfiles
