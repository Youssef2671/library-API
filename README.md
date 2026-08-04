# Library Management System API

## Overview
A robust and secure RESTful API built with ASP.NET Core for managing library resources. This backend service handles user authentication, book cataloging, and author management, serving as the foundation for a Single Page Application (SPA) dashboard. 

## Technologies & Tools
- Framework: ASP.NET Core Web API (.NET)
- Language: C#
- Database: Microsoft SQL Server
- ORM: Entity Framework Core
- Authentication: JWT (JSON Web Tokens)
- Security: BCrypt.Net-Next (Password Hashing), CORS Policy configured
- Data Mapping: AutoMapper
- Validation: FluentValidation

## Architecture & Design Patterns
- Repository Pattern: Implemented for clean data access and separation of concerns.
- DTOs (Data Transfer Objects): Used to encapsulate data payloads and protect internal database models.
- Global Error Handling: Centralized exception management to provide consistent and secure API responses.

## Key Features
- Authentication & Authorization: Secure user registration and login utilizing BCrypt for password hashing and JWT for route protection.
- Books Management: Full CRUD operations for books, protected by authorization.
- Authors Management: Full CRUD operations for authors, linked relationally to books.
- Public & Private Routes: Read-only access for public users, while data modification requires a valid Bearer Token.

## Getting Started

### Prerequisites
- .NET SDK (Version 6.0 or later)
- SQL Server
- Visual Studio / Visual Studio Code or any preferred IDE

### Installation & Setup

1. Clone the repository:
   ```bash
   git clone [https://github.com/YourUsername/library-API.git](https://github.com/YourUsername/library-API.git)
   ```

2. Navigate to the project directory:
   ```bash
   cd library-API
   ```

3. Configuration:
   Open the `appsettings.json` file and update the following:
   - `ConnectionStrings`: Point it to your local SQL Server instance.
   - `Jwt`: Update the `Key` with a strong, secure secret string.

4. Database Migration:
   Apply the migrations to generate the database tables by running the following command in your terminal:
   ```bash
   dotnet ef database update
   ```

5. Run the Application:
   ```bash
   dotnet run
   ```
   The API will start, and you can explore the endpoints using tools like Postman or the built-in Swagger UI interface.

## API Endpoints Summary

### Auth
- `POST /api/auth/register` - Create a new user account.
- `POST /api/auth/login` - Authenticate user and generate JWT.

### Books
- `GET /api/books` - Retrieve the list of all books.
- `POST /api/books` - Add a new book (Requires Token).
- `PUT /api/books/{id}` - Update book details (Requires Token).
- `DELETE /api/books/{id}` - Delete a book (Requires Token).

### Authors
- `GET /api/authors` - Retrieve the list of all authors.
- `POST /api/authors` - Add a new author (Requires Token).
- `PUT /api/authors/{id}` - Update author details (Requires Token).
- `DELETE /api/authors/{id}` - Delete an author (Requires Token).
