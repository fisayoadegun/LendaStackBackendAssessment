# LendaStackBackendAssessment - LendaStack Currency Converter

## 📖 Overview

The **LendaStack Currency Converter** is a robust and extensible API built with **.NET 8**, designed to provide real-time and historical currency conversion services. The project is built using a **clean architecture** pattern and follows best practices to ensure scalability, maintainability, and developer productivity.

---

## ✨ Highlights

- **Clean Architecture:** Layered architecture ensuring separation of concerns, testability, and scalability.
- **Security Middleware:** Middleware-based token validation and authorization to secure endpoints.
- **Global Exception Handler:** Centralized error handling with consistent error responses for improved developer experience.
- **EF Core + SQL Server:** Uses Entity Framework Core with SQL Server for reliable data access and storage.
- **FluentValidation:** Applies strong validation rules on all request models using FluentValidation.
- **MediatR:** Leverages MediatR for CQRS implementation and decoupled command/query logic.
- **AutoMapper:** Simplifies DTO-to-entity conversions and vice versa.
- **Serilog Logging:** Structured and file-based logging using Serilog for tracking application behavior and diagnostics.
- **Unit & Integration Tests:** Comprehensive test suite using xUnit to ensure correctness and reliability.
- **Performance Middleware:** Tracks execution time of requests and flags long-running operations.
- **Data Seeding:** Pre-populated dummy data for test and demo purposes.

---

## 🧪 Dummy API Key Credentials

Use these test ApiKey to simulate authenticated operations (if applicable):

- APIKey: test-key-123

---

## 🚀 Getting Started

1. **Clone the repository**
   ```bash
   git clone https://github.com/fisayoadegun/LendaStackBackendAssessment.git
2. **Update the connection string**
    Navigate to appsettings.json and modify the SQL Server connection string to point to your local or remote DB instance.
3.  **Run migrations and seed data**
4. **Build and run**
     ```bash
     dotnet build
    dotnet run --project src/LendaStackCurrencyConverter.API
5. **Explore Swagger**
   Navigate to https://localhost:port/swagger to explore and test API endpoints.

🎯 Project Scope
This project is part of a Take Home Assessment focused on demonstrating backend engineering capabilities.

✅ Features Implemented:
Fetch and update historical and real-time exchange rates.

Rate limiting and API key support (extensible design).

Background jobs for periodic data fetching (if applicable).

Error handling and performance monitoring.

Functional and integration test coverage.

📦 Tech Stack
.NET 8

Entity Framework Core

SQL Server

MediatR

FluentValidation

AutoMapper

xUnit

Serilog

