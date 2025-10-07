# shop-demo

Shop-demo is a small example microservices-based storefront that demonstrates a full-stack setup using:

- ASP.NET Core (.NET 8) microservices for products and orders
- Ocelot API Gateway to route and secure requests
- Angular (v20) frontend with optional SSR
- Docker support for containerized development

This repository is organized to show separation of concerns for API, service, domain and infrastructure layers and includes an API gateway and a front-end application.

## Repository layout

- `api/`: Multiple .NET solutions/projects for orders and products (API, service, domain, infrastructure)
  - `example.order.api/` - Order API (ASP.NET Core)
  - `example.product.api/` - Product API (ASP.NET Core)
  - Service, domain and infrastructure projects follow a layered architecture
- `gateway/`: Ocelot API Gateway that aggregates and routes requests to backend services
- `fe/`: Angular front-end application (example)

## Key technologies

- .NET 8 (ASP.NET Core)
- Ocelot API Gateway
- Angular 20 (with server-side rendering support)
- Docker (Dockerfiles provided)

## Prerequisites

- .NET 8 SDK (https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- Node.js (v18+) and npm (for the Angular frontend)
- Angular CLI (optional; install with `npm i -g @angular/cli`)
- Docker (for containerized development)
- Windows PowerShell (instructions assume PowerShell; adjust for other shells)

## Local development — quick start (PowerShell)

Follow these steps to run the services locally during development. Open separate PowerShell tabs for each long-running process.

1) Start backend APIs

The API projects target .NET 8. Example: start the order and product APIs.

Order API:

```powershell
cd .\api\example.order.api\example.order.api
dotnet run
```

Product API:

```powershell
cd .\api\example.product.api\example.product.api
dotnet run
```

Note: The gateway configuration (`gateway/example.api.gateway/ocelot.json`) expects the order API at http://localhost:5018 and the product API at https://localhost:5002. If your local runs use different ports or schemes, update `ocelot.json` or your launch settings to match.

2) Run the API gateway

Open a new PowerShell tab and run:

```powershell
cd .\gateway\example.api.gateway
dotnet run
```

The gateway loads `ocelot.json` and forwards requests to the downstream services. It also prints simple request logs by default.

3) Run the frontend (Angular)

From the frontend folder:

```powershell
cd .\fe\example
npm install
npm start
```

This starts the Angular development server (default: http://localhost:4200). You may need to configure the frontend to point to the gateway URLs or add a proxy configuration depending on how the front-end environment is set up.

Optional: SSR

The frontend contains an SSR build/serve script (`serve:ssr:example`) for server-side rendering. Build the app for production and then run the server bundle if you need SSR in a test environment.

## Running with Docker

Each service contains a Dockerfile. To run services in containers locally, build and run images for each service.

Example — build and run product API image:

```powershell
cd .\api\example.product.api\example.product.api
docker build -t example-product:local .
docker run --rm -p 5002:5002 --name example-product example-product:local
```

Repeat for the order API and gateway. When running containers, ensure network/port mappings match the downstream hosts/ports in `gateway/example.api.gateway/ocelot.json`. Using a user-defined Docker network or a `docker-compose.yml` (recommended) simplifies container-to-container communication.

## Configuration

- Gateway routing is defined in `gateway/example.api.gateway/ocelot.json` (routes for orders and products are present).
- Each API project has `appsettings.json` and (optionally) `appsettings.Development.json` for environment-specific settings.

## Troubleshooting

- Port conflicts: If `dotnet run` fails due to port in use, check the project's `Properties/launchSettings.json` or console output for which URLs/ports the app is bound to. Update `ocelot.json` or your launch profile if you change ports.
- HTTPS vs HTTP: Ocelot routes may use `https` for some downstream services. For local development you can either run the downstream service with HTTPS enabled or modify `ocelot.json` to use `http` (not recommended for production).
- CORS issues: The gateway in source enables a permissive CORS policy. If the frontend still cannot reach the gateway, verify the gateway is running and that the frontend is sending requests to the correct gateway address/port.

## Suggested next steps

- Add a `docker-compose.yml` to orchestrate the gateway, backend services and frontend for a single-command local start.
- Add CI (GitHub Actions) to build and run tests for each project on push/PR.
- Add a `docs/` folder with API endpoint examples, sample requests, and architecture diagrams.

## Contributing

Contributions are welcome. For larger changes, open an issue first to discuss the design. Include tests and update documentation for any new behavior.

## License

This repository is provided for demo and educational purposes. Add a license file if you plan to publish or share widely.

---

If you want, I can also:

- Add a `docker-compose.yml` that wires the gateway, product and order APIs and the frontend for a one-command local start.
- Expand this README with example API endpoints and sample curl requests.

Tell me which of those you'd like next.