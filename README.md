<div align="center">

# 💰 FinanceAPI

### Personal Finance & Expense Tracker — REST API

*A production-structured ASP.NET Core Web API built with clean layered architecture, JWT authentication, and ownership-based access control.*

![.NET](https://img.shields.io/badge/.NET_8-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![ASP.NET Core](https://img.shields.io/badge/ASP.NET_Core-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![Entity Framework](https://img.shields.io/badge/Entity_Framework_Core-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![SQL Server](https://img.shields.io/badge/SQL_Server-CC2927?style=for-the-badge&logo=microsoftsqlserver&logoColor=white)
![JWT](https://img.shields.io/badge/JWT-000000?style=for-the-badge&logo=jsonwebtokens&logoColor=white)

</div>

---

## 📌 Overview

FinanceAPI is a RESTful backend API that lets users track personal income and expenses. Built to follow enterprise-grade architectural patterns, it separates concerns across a **two-project solution** — a dedicated Data Access Layer and a Web API layer — following the **Controller → Service → Repository → Database** pipeline.

This is not a tutorial clone. Every architectural decision was made intentionally: from extracting `UserId` from JWT claims instead of the request body, to physically separating the DAL into its own class library project.

---

## 🏗️ Solution Architecture

```
Solution/
├── DAL/                          # Class Library — Data Access Layer
│   ├── Data/
│   │   └── AppDbContext.cs
│   ├── Models/
│   │   ├── User.cs
│   │   ├── Category.cs
│   │   └── Transaction.cs
│   ├── Interfaces/
│   │   ├── IUserRepository.cs
│   │   ├── ICategoryRepository.cs
│   │   └── ITransactionRepository.cs
│   ├── Repositories/
│   │   ├── UserRepository.cs
│   │   ├── CategoryRepository.cs
│   │   └── TransactionRepository.cs
│   └── Migrations/
│
└── FinanceAPI/                   # ASP.NET Core Web API
    ├── Controllers/
    ├── DTOs/
    ├── Services/
    │   ├── ITransactionService.cs
    │   └── TransactionService.cs
    ├── Middleware/
    ├── Program.cs
    └── appsettings.json
```

### Why Two Projects?

The DAL is a **separate Class Library**, not a folder inside the API project. This means:
- The data layer has zero dependency on ASP.NET Core
- Business logic in the API cannot accidentally couple to HTTP concerns
- The DAL can be reused by any future consumer (gRPC service, console app, background worker)

---

## ⚙️ Tech Stack

| Layer | Technology |
|---|---|
| Framework | ASP.NET Core Web API (.NET 8) |
| ORM | Entity Framework Core |
| Database | SQL Server |
| Authentication | JWT Bearer Tokens |
| Password Security | BCrypt.Net |
| API Documentation | Scalar |
| Architecture | Repository Pattern + Service Layer |

---

## 🔐 Security Design

Security was a first-class concern, not an afterthought.

- **BCrypt hashing** — passwords are never stored in plain text; BCrypt with salt is used for both hashing and verification
- **JWT Claims-based identity** — `UserId` is extracted from the JWT `NameIdentifier` claim server-side; it is never trusted from the request body
- **Ownership validation** — every transaction read, update, and delete checks that the requesting user owns the resource before proceeding
- **Protected endpoints** — all transaction endpoints require a valid Bearer token via `[Authorize]`

```
Claims stored in JWT:
  ├── NameIdentifier  →  UserId
  └── Email           →  User Email
```

---

## 🗃️ Database Schema

```
┌─────────────┐         ┌──────────────────┐         ┌─────────────┐
│    User     │         │   Transaction    │         │  Category   │
├─────────────┤         ├──────────────────┤         ├─────────────┤
│ Id          │◄────────│ UserId (FK)      │         │ Id          │
│ Name        │         │ CategoryId (FK)  │────────►│ Name        │
│ Email       │         │ Id               │         │ Type        │
│ Password    │         │ Amount           │         └─────────────┘
│ CreatedAt   │         │ Description      │
└─────────────┘         │ TransactionDate  │
                        │ TransactionType  │
                        └──────────────────┘
```

Relationships configured using **EF Core Fluent API** (not data annotations).

---

## 📡 API Endpoints

### 🔑 Auth
| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| `POST` | `/api/auth/login` | Login and receive JWT token | ✅ |

### 👤 Users
| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| `POST` | `/api/users` | Register a new user | ✅ |
| `GET` | `/api/users` | Get all users | ✅ |
| `GET` | `/api/users/{id}` | Get user by ID | ✅ |
| `PUT` | `/api/users/{id}` | Update user | ✅ |
| `DELETE` | `/api/users/{id}` | Delete user | ✅ |

### 🏷️ Categories
| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| `POST` | `/api/categories` | Create a category | ✅ |
| `GET` | `/api/categories` | Get all categories | ✅ |
| `GET` | `/api/categories/{id}` | Get category by ID | ✅ |
| `PUT` | `/api/categories/{id}` | Update category | ✅ |
| `DELETE` | `/api/categories/{id}` | Delete category | ✅ |

### 💸 Transactions
| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| `POST` | `/api/transactions` | Create a transaction | ✅ |
| `GET` | `/api/transactions` | Get all transactions | ✅ |
| `GET` | `/api/transactions/{id}` | Get transaction by ID | ✅ |
| `PUT` | `/api/transactions/{id}` | Update transaction | ✅ |
| `DELETE` | `/api/transactions/{id}` | Delete transaction | ✅ |
| `GET` | `/api/transactions/my` | Get **my** transactions (from JWT) | ✅ |

> ✅ = Requires `Authorization: Bearer <token>` header

---

## 🔄 Request Flow

```
HTTP Request
     │
     ▼
 Controller          — Handles HTTP input/output only
     │
     ▼
  Service            — Business logic, ownership validation
     │
     ▼
 Repository          — Data access via EF Core
     │
     ▼
  Database           — SQL Server
```

No business logic lives in Controllers. No database calls live in Services. Each layer has one responsibility.

---

## 🚀 Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (or SQL Server Express)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or [VS Code](https://code.visualstudio.com/)

### 1. Clone the Repository

```bash
git clone https://github.com/yourusername/FinanceAPI.git
cd FinanceAPI
```

### 2. Configure the Database

Update `appsettings.json` in the `FinanceAPI` project:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER;Database=FinanceDB;Trusted_Connection=True;TrustServerCertificate=True"
  },
  "JwtSettings": {
    "Key": "your-secret-key-min-32-characters-long",
    "Issuer": "FinanceAPI",
    "Audience": "FinanceAPIUsers"
  }
}
```

### 3. Apply Migrations

```bash
cd DAL
dotnet ef database update --startup-project ../FinanceAPI
```

### 4. Run the API

```bash
cd FinanceAPI
dotnet run
```

### 5. Open Scalar Docs

Navigate to `https://localhost:{port}/scalar` to explore and test all endpoints interactively.

---

## 🧪 Example: Authenticate and Create a Transaction

**Step 1 — Login**
```http
POST /api/auth/login
Content-Type: application/json

{
  "email": "user@example.com",
  "password": "yourpassword"
}
```

**Response**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
}
```

**Step 2 — Create a Transaction**
```http
POST /api/transactions
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
Content-Type: application/json

{
  "amount": 1500.00,
  "description": "Grocery shopping",
  "transactionDate": "2025-06-08",
  "transactionType": "Expense",
  "categoryId": 3
}
```

> `UserId` is **not** in the request body. It is extracted server-side from the JWT claim.

---

## 🗺️ Roadmap

- [x] Full CRUD — Users, Categories, Transactions
- [x] JWT Authentication & Token Generation
- [x] BCrypt Password Hashing
- [x] Ownership-based Access Control
- [x] Repository Pattern
- [x] Service Layer
- [x] Two-project Solution Architecture
- [ ] Global Exception Handling Middleware
- [ ] FluentValidation on all DTOs
- [ ] Response DTOs for all endpoints
- [ ] AutoMapper
- [ ] Monthly Summary Report API
- [ ] Category-wise Spending Report
- [ ] Pagination on list endpoints
- [ ] Unit Tests

---

## 🧠 Key Design Decisions

**Why is DAL a separate project and not a folder?**
Physical separation enforces architectural boundaries at the compiler level. The API project cannot bypass the repository layer — there's no direct path to `AppDbContext` from a Controller.

**Why is UserId taken from JWT claims and not the request body?**
Trusting `userId` from the client is a broken access control vulnerability (OWASP A01). The server is the only authority on who is making the request.

**Why Fluent API over Data Annotations for relationships?**
Data annotations mix infrastructure concerns into domain models. Fluent API keeps models clean and configuration explicit and centralized.

---

## 👨‍💻 Author

**Siddhanth Chabri**

[![GitHub](https://img.shields.io/badge/GitHub-181717?style=for-the-badge&logo=github&logoColor=white)](https://github.com/yourusername)
[![LinkedIn](https://img.shields.io/badge/LinkedIn-0A66C2?style=for-the-badge&logo=linkedin&logoColor=white)](https://linkedin.com/in/yourprofile)

---

<div align="center">

*Built with intent. Not from a tutorial.*

</div>
