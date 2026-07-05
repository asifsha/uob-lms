# 📚 Library Management System (LMS) – Web API

A modular Library Management System built using ASP.NET Core (.NET 8) with clean architecture principles, JWT authentication, and layered design.

This project demonstrates real-world backend development concepts including authentication, repository pattern, dependency injection, EF Core with SQLite, and integration testing.

---

## 🚀 Tech Stack

* **Runtime:** .NET 8 (ASP.NET Core Web API)
* **ORM:** Entity Framework Core
* **Database:** SQLite (file-based database)
* **Object Mapping:** AutoMapper
* **Security:** JWT Authentication (Bearer Tokens)
* **Password Security:** Password Hashing (ASP.NET Identity PasswordHasher)
* **Testing Framework:** xUnit (Integration Testing)
* **Assertions:** FluentAssertions
* **Test Host:** WebApplicationFactory (End-to-End API Testing)

---

## 🏗️ Architecture

The solution follows a layered architecture to maintain a clean separation of concerns:

* `lms-api` → **Presentation Layer** (Controllers, Program.cs)
* `lms-service` → **Business Logic Layer** (Services, DTOs, Auth logic)
* `lms-data` → **Data Access Layer** (EF Core, Entities, Repositories)
* `lms-api-tests` → **Integration Tests** (End-to-End API testing)

---

## 🧩 Key Features

### 📖 Library Features
* Manage Books (Create, Update, Delete, Get)
* Manage Members
* Borrow & Return Books
* Track overdue records

### 🔐 Authentication & Authorization
* User registration & login
* JWT token generation
* Role-based access control (Admin / Member)
* Secure endpoints using `[Authorize]`

### 🧠 Clean Code Practices
* Repository Pattern
* Dependency Injection
* Service Layer abstraction
* DTO / Business Model separation
* AutoMapper for object mapping

### 🗄️ Database
* SQLite database (`library.db`)
* EF Core Code-First migrations
* Relationships:
  * Library → Books
  * Books → Borrow Records
  * Members → Borrow Records

---

## 🔄 API Endpoints (Sample)

### Auth
* `POST /api/auth/register`
* `POST /api/auth/login`

### Books
* `GET    /api/books`
* `POST   /api/books`
* `PUT    /api/books/{id}`
* `DELETE /api/books/{id}`

### Members
* `GET    /api/members`
* `POST   /api/members`

### Borrow System
* `POST   /api/borrow`
* `POST   /api/borrow/return`
* `GET    /api/borrow/overdue`

---

## 🧪 Testing

Integration tests cover full API workflows:
* User registration & login flow
* JWT authentication validation
* Book creation and retrieval
* Borrowing workflow end-to-end

**Tools used:**
* xUnit
* WebApplicationFactory
* In-memory test database
* FluentAssertions

---

## 🔐 Security

* Passwords are securely hashed using `PasswordHasher<T>`
* JWT-based authentication
* Role-based authorization support
* Protected API endpoints using `[Authorize]`

---

## ⚙️ How to Run

### 1. Run API
```bash
dotnet run --project lms-api
```

### 2. Apply migrations
```bash
dotnet ef database update --project lms-data --startup-project lms-api
```

### 3. Run tests
```bash
dotnet test
```

---

## 📌 Highlights

* Clean separation of concerns (API / Service / Data)
* Real-world authentication system (JWT)
* Fully working relational database model
* End-to-end automated API tests
* Production-style architecture patterns

---
