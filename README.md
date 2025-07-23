# CosmosDB .NET CRUD Example

This project demonstrates basic CRUD (Create, Read, Update, Delete) operations using Azure Cosmos DB with a .NET application.

## Features
- Connects to Azure Cosmos DB
- Implements CRUD operations for items
- Organized with Controllers, Entities, and Services

## Project Structure
- `Controllers/ItemsController.cs`: API controller for item operations
- `Entities/Item.cs`: Item entity definition
- `Service/CosmosDbService.cs`: Service for Cosmos DB interactions
- `IService/ICosmosDbService.cs`: Service interface
- `Program.cs`: Application entry point
- `appsettings.json`: Configuration settings

## Getting Started

### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Azure Cosmos DB account

### Configuration
1. Update `appsettings.json` and `appsettings.Development.json` with your Cosmos DB connection string and database/container names.

### Build and Run
```powershell
# Restore dependencies
 dotnet restore

# Build the project
 dotnet build

# Run the application
 dotnet run
```

## Usage
- The API exposes endpoints for CRUD operations on items.
- Use tools like [Postman](https://www.postman.com/) or `CosmosDB.http` for testing HTTP requests.

## License
This project is licensed under the MIT License.
