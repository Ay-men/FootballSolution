# Football Transfer Management API

A .NET Core API for managing football player transfers and contracts.

## Features

- Player management (CRUD operations)
- Team management 
- Contract management
- Transfer handling
- Market value tracking

## Prerequisites

- Docker Desktop
- .NET 8.0 SDK (for local development)

## Getting Started

1. Clone the repository
2. Navigate to the project root directory
3. Run with Docker Compose:

```bash
docker compose up -d
```

This will start:
- SQL Server container
- API container
- Redis cache container

The API will be available at `http://localhost:5218/swagger/index.html`

## API Endpoints

### Players
- GET `/api/players/{id}` - Get player by ID
- POST `/api/players` - Create new player
- PUT `/api/players/{id}` - Update player

### Teams
- GET `/api/teams/{id}` - Get team by ID
- POST `/api/teams` - Create new team
- GET `/api/teams/{id}/players` - Get team's players

### Transfers
- POST `/api/transfers` - Initiate transfer
- GET `/api/transfers/{id}` - Get transfer details
