# Flower Shop API

A C# backend server application for a Flower Shop service that provides APIs for order management.

## Features

- Create, update, retrieve, list, and cancel flower shop orders
- RESTful API design
- SQL Server database integration
- Comprehensive unit tests

## API Endpoints

- `POST /api/orders` - Create a new order
- `GET /api/orders` - List all orders
- `GET /api/orders/{id}` - Get a specific order by ID
- `PUT /api/orders/{id}` - Update an existing order
- `POST /api/orders/{id}/cancel` - Cancel an order

## Project Structure

- **FlowerShopAPI** - Main API project
  - **Controllers** - API controllers
  - **Models** - Data models
  - **Data** - Database context and configuration
  - **Repositories** - Data access layer
- **FlowerShopAPI.Tests** - Unit tests
  - **Controllers** - Controller tests
  - **Models** - Model tests
  - **Repositories** - Repository tests

## Technologies Used

- ASP.NET Core 7.0
- Entity Framework Core
- SQL Server
- xUnit for testing
- Moq for mocking

## Getting Started

### Prerequisites

- .NET 7.0 SDK
- SQL Server (local or remote)

### Setup

1. Clone the repository
2. Update the connection string in `appsettings.json` to point to your SQL Server instance
3. Run database migrations:
   ```
   dotnet ef database update
   ```
4. Run the application:
   ```
   dotnet run --project FlowerShopAPI
   ```

### Running Tests

```
dotnet test
```

## API Usage Examples

### Create Order

```http
POST /api/orders
Content-Type: application/json

{
  "customerName": "John Doe",
  "customerEmail": "john@example.com",
  "totalAmount": 75.50,
  "items": [
    {
      "flowerName": "Red Rose",
      "quantity": 5,
      "unitPrice": 10.00
    },
    {
      "flowerName": "White Lily",
      "quantity": 3,
      "unitPrice": 8.50
    }
  ]
}
```

### Get Order

```http
GET /api/orders/1
```

### Update Order

```http
PUT /api/orders/1
Content-Type: application/json

{
  "customerName": "John Doe",
  "customerEmail": "john.updated@example.com",
  "totalAmount": 95.50,
  "items": [
    {
      "flowerName": "Red Rose",
      "quantity": 7,
      "unitPrice": 10.00
    },
    {
      "flowerName": "White Lily",
      "quantity": 3,
      "unitPrice": 8.50
    }
  ]
}
```

### Cancel Order

```http
POST /api/orders/1/cancel
```