# Using CosmosDB in .NET 8 for Scalable NoSQL Applications

This project demonstrates how to build a scalable **.NET 8** Web API application using **Azure Cosmos DB** for storing and retrieving NoSQL data.  
It includes fully implemented CRUD (**Create**, **Read**, **Update**, **Delete**) operations with a clean and maintainable architecture.

---

## Features
- Connects to Azure Cosmos DB (Core SQL API)
- Implements CRUD operations for items
- Organized with Controllers, Entities, and Services
- Configuration-driven connection settings
- Swagger UI for API exploration

---

## Project Structure
```
Controllers/
 └── ItemsController.cs        # API controller for item operations
Entities/
 └── Item.cs                   # Item entity definition
IService/
 └── ICosmosDbService.cs       # Service interface
Service/
 └── CosmosDbService.cs        # Cosmos DB service implementation
Program.cs                     # Application entry point
appsettings.json               # Production configuration
appsettings.Development.json   # Development configuration
```

---

## Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Azure Cosmos DB account (Core SQL API)

---

## Required NuGet Packages
Install the official Cosmos DB SDK:

```powershell
dotnet add package Microsoft.Azure.Cosmos
```

---

## Azure Cosmos DB Setup

1. **Create Cosmos DB account**
   - Go to the [Azure Portal](https://portal.azure.com/)
   - **Create a resource** → Search for **Azure Cosmos DB** → Select **Create**
   - Choose **Core (SQL)** API
   - Fill in resource group, account name, and region
   - Click **Review + Create** → **Create**
   
2. **Create Database & Container**
   - Open your Cosmos DB account
   - Go to **Data Explorer**
   - Click **New Database** → Enter a **Database ID**
   - Click **New Container** → Enter **Container ID** and **Partition Key** (e.g., `/id`)
   
3. **Get Connection Credentials**
   - Go to **Keys** in the left menu
   - Copy **URI** and **Primary Key**

---

## Configuration
Update `appsettings.json` and/or `appsettings.Development.json` with your Cosmos DB details:

```json
"CosmosDb": {
  "Account": "https://<your-account>.documents.azure.com:443/",
  "Key": "<your-primary-key>",
  "DatabaseName": "MyDatabase",
  "ContainerName": "Items"
}
```

---

## Service Registration
In `Program.cs`, register the CosmosDB client and service in the dependency injection (DI) container:

```csharp
var builder = WebApplication.CreateBuilder(args);

// Access configuration  
var configuration = builder.Configuration;
var cosmosSection = configuration.GetSection("CosmosDb");
string account = cosmosSection["Account"]!;
string key = cosmosSection["Key"]!;
string dbName = cosmosSection["DatabaseName"]!;
string contName = cosmosSection["ContainerName"]!;

// Register CosmosClient and CosmosDbService  
builder.Services.AddSingleton(s => new CosmosClient(account, key));
builder.Services.AddSingleton<ICosmosDbService>(s =>
{
    var client = s.GetRequiredService<CosmosClient>();
    var logger = s.GetRequiredService<ILogger<CosmosDbService>>();
    return new CosmosDbService(client, dbName, contName, logger);
});

builder.Services.AddControllers();
```

## Build and Run

```powershell
# Restore dependencies
dotnet restore

# Build the project
dotnet build

# Run the application
dotnet run
```

---

## API Endpoints

| Method | Endpoint          | Description                              |
|--------|-------------------|------------------------------------------|
| GET    | `/api/items`      | Retrieve all items (with pagination support) |
| GET    | `/api/items/{id}` | Get a single item by ID                   |
| POST   | `/api/items`      | Create a new item                         |
| PUT    | `/api/items/{id}` | Update an existing item                   |
| DELETE | `/api/items/{id}` | Delete an item                            |

You can explore these endpoints via:
- **Swagger UI** (`https://localhost:<port>/swagger`)
- **Postman**
- **CosmosDB.http** (optional)

---
## Exploring API Endpoints in Swagger

![Exploring API Endpoints in Swagger](assets/swagger-demo.gif)

---

## Performance Tips
- Choose a Partition Key carefully for scalability.
- Use Throughput (RU/s) settings that match your workload.
- Enable or fine-tune the Indexing Policy for optimal query speed.
- Avoid cross-partition queries when possible.

## Security Notes
- Never commit your Primary Key to version control.
- Use Azure Key Vault or environment variables for secrets.
- Enable Role-Based Access Control (RBAC) for Cosmos DB.