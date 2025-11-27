# SmartSchool – .NET 10 Clean Architecture (Minimal API + CQRS + EF Core)

SmartSchool is a production-grade school management backend built with **.NET 10**, **Clean Architecture**, **CQRS with MediatR**, **EF Core**, and **Minimal APIs**.  
It demonstrates professional enterprise patterns suitable for real-world projects and showcases your architectural skills to clients.

## ✨ Features
- .NET 10 Minimal API + Clean Architecture
- Domain Driven folder structure
- CQRS (Commands / Queries) using MediatR
- EF Core 10 (SQL Server)
- FluentValidation for request validation
- FluentResults for uniform success/failure responses
- Serilog logging
- Modular endpoints (Students, Teachers, Classes, Auth, Attendance)
- Ready for microservices expansion
- Unit tests (xUnit + FluentAssertions)

---

## 🏗️ Architecture Overview
Api → Application → Domain → Infrastructure → Shared

API Layer

Minimal API endpoints

Authentication / Authorization

Serilog request logging

Swagger & API versioning

Application Layer

CQRS (Commands & Queries)

Handlers, Validators, DTOs

No EF Core dependency

Business rules

Domain Layer

Entities

Business objects only

No infrastructure or framework dependency

Infrastructure Layer

EF Core DbContext

Repositories

External services


---

## 🗂️ Project Structure

```
SmartSchool/
│
├── src/
│   ├── SmartSchool.Api/
│   ├── SmartSchool.Application/
│   ├── SmartSchool.Domain/
│   ├── SmartSchool.Infrastructure/
│   └── SmartSchool.Shared/
│
└── tests/
    └── SmartSchool.Tests/
```

---

## 🚀 Getting Started

### 1. Restore & Build
dotnet restore
dotnet build


### 2. Update Connection String
`appsettings.json`


"ConnectionStrings": {
"DefaultConnection": "Server=.;Database=SmartSchoolDb;Trusted_Connection=True;TrustServerCertificate=True;"
}


### 3. Apply EF Migrations


dotnet ef migrations add InitialCreate -p src/SmartSchool.Infrastructure -s src/SmartSchool.Api
dotnet ef database update -p src/SmartSchool.Infrastructure -s src/SmartSchool.Api


### 4. Run the API


dotnet run --project src/SmartSchool.Api


Swagger UI will be available at:


https://localhost:<port>/swagger


---

## 📦 Main NuGet Packages

| Package | Reason | Benefit |
|--------|--------|---------|
| MediatR | CQRS | Cleaner separation, scalable handlers |
| FluentValidation | Input validation | Declarative rules, reusable |
| EF Core 10 | ORM | SQL support & migrations |
| FluentResults | Success/Fail standard | Replaces exceptions for flow |
| Serilog + Seq | Logging | Structured logs, observability |
| Swashbuckle | API docs | Swagger UI generation |

---

## 🧪 Testing



dotnet test


Tests use:
- xUnit
- FluentAssertions
- Moq (mocking)

---

## 📄 License
MIT

---

