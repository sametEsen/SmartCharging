# **Smart Charging API** 🚗⚡  

This project is a **DDD-based** .NET Web API for managing **charging stations, groups, and connectors**.  

## **📖 Table of Contents**
- [Features](#-features)
- [Technologies Used](#-technologies-used)
- [Architecture](#-architecture)
- [Installation](#-installation)
- [Database Setup](#-database-setup)
- [API Endpoints](#-api-endpoints)
- [Swagger UI](#-swagger-ui)
- [Validation](#-validation)
- [Unit Testing](#-unit-testing)

---

## **🚀 Features**
✅ Create, update, and delete **Groups, Charge Stations, and Connectors**  
✅ Enforce **business rules** (e.g., a Charge Station must belong to **one Group**)  
✅ Prevent **exceeding Group’s capacity** with Connector updates  
✅ Implement **Unit of Work (UoW)** and **Generic Repositories**  
✅ Validate API requests using **FluentValidation**  
✅ Provide **comprehensive unit tests** using **NUnit & Moq**  
✅ **Swagger UI** for API documentation and testing  

---

## **🛠️ Technologies Used**
| Technology                | Purpose |
|---------------------------|---------|
| **.NET 7 / .NET 8**       | Backend Framework |
| **ASP.NET Core Web API**  | REST API |
| **Entity Framework Core** | ORM |
| **In-Memory Database**    | Database (for now) |
| **AutoMapper**            | DTO Mapping |
| **FluentValidation**      | Request Validation |
| **NUnit & Moq**           | Unit Testing |
| **Swagger UI**            | API Documentation |

---

## **📂 Architecture**
This project follows **Domain-Driven Design (DDD)** principles.

```
SmartCharging/
│── SmartCharging.Api/              # API Layer (Controllers, Startup)
│── SmartCharging.Application/      # Application Layer (Services, DTOs)
│── SmartCharging.Domain/           # Domain Layer (Entities, Business Logic)
│── SmartCharging.Infrastructure/   # Infrastructure Layer (Repositories, DbContext)
│── SmartCharging.Tests/            # Unit Tests (NUnit)
```

**Key Components:**
- **Entities**: `Group`, `ChargeStation`, `Connector`
- **Repositories**: `IGenericRepository<T>`, `IGroupRepository`, etc.
- **Services**: `GroupService`, `ChargeStationService`, `ConnectorService`
- **Validators**: `GroupValidator`, `ConnectorValidator`
- **DTOs**: `GroupDto`, `UpdateConnectorDto`, etc.

---

## **📥 Installation**
1️⃣ **Install Dependencies**  
```sh
dotnet restore
```

3️⃣ **Run the API**  
```sh
dotnet run --project SmartCharging.Api
```
🚀 The API will be available at `http://localhost:5075`

---

## **🗄️ Database Setup**
This API currently uses **Entity Framework Core In-Memory Database**.  
You can change it to **SQL Server** by updating `SmartChargingContext.cs` in **Infrastructure Layer**.

---

## **📡 API Endpoints**
All responses follow **RESTful conventions** and use **DTOs**.  

### **📌 Group Endpoints**
| Method | Endpoint                | Description |
|--------|-------------------------|-------------|
| **GET**  | `/api/groups`         | Get all groups |
| **GET**  | `/api/groups/{id}`    | Get a group by ID |
| **POST** | `/api/groups`         | Create a new group |
| **PUT**  | `/api/groups/{id}`    | Update a group |
| **DELETE** | `/api/groups/{id}` | Delete a group (deletes all related ChargeStations) |

---

### **📌 Charge Station Endpoints**
| Method | Endpoint                        | Description |
|--------|---------------------------------|-------------|
| **GET**  | `/api/charge-stations`       | Get all charge stations |
| **GET**  | `/api/charge-stations/{id}`  | Get a charge station by ID |
| **POST** | `/api/charge-stations`       | Create a new charge station |
| **PUT**  | `/api/charge-stations/{id}`  | Update a charge station |
| **DELETE** | `/api/charge-stations/{id}` | Delete a charge station (deletes all related Connectors) |

---

### **📌 Connector Endpoints**
| Method | Endpoint                               | Description |
|--------|---------------------------------------|-------------|
| **GET**  | `/api/connectors`                  | Get all connectors |
| **GET**  | `/api/connectors/{stationId}/{id}` | Get a connector by ID |
| **POST** | `/api/connectors`                  | Create a new connector |
| **PUT**  | `/api/connectors/{stationId}/{id}` | Update a connector |
| **DELETE** | `/api/connectors/{stationId}/{id}` | Delete a connector |

---

## **📜 Swagger UI**
The project includes **Swagger UI** for testing the API easily.  

To use Swagger UI, run the application and navigate to:  
📌 `http://localhost:5075/swagger/index.html`  

Swagger will display all API endpoints and allow you to **test them directly**.

---

## **✅ Validation (FluentValidation)**
| Rule | Validation |
|------|-----------|
| **Group Capacity** | Must be **greater than or equal to** the total Amps of its Connectors |
| **Charge Station Rules** | Must have **1-5 Connectors** |
| **Connector Max Current** | Must be **> 0**, and **cannot exceed Group’s capacity** |

Example: **Reject Connector Update if It Exceeds Group Capacity**
```json
{
  "error": "Updating max current exceeds the group's capacity."
}
```

---

## **🧪 Unit Testing**
**Test Framework:** `NUnit + Moq + FluentAssertions`  

Run tests:  
```sh
dotnet test
```

### **📌 Unit Tests Implemented**
✔️ **GroupControllerTests**  
✔️ **ChargeStationControllerTests**  
✔️ **ConnectorControllerTests**  

---

## **📌 Final Notes**
This project follows **clean architecture** and **DDD principles** while enforcing **business rules strictly**. 🚀   
