# Athenaeum - Library Management System API

## Overview
A robust, secure, and enterprise-ready RESTful API built with ASP.NET Core for managing library resources. This backend service handles user authentication, advanced book cataloging, and author management, serving as a solid foundation for a modern Single Page Application (SPA) dashboard.

## Technologies & Tools
- Framework: ASP.NET Core Web API (.NET 8.0)
- Language: C#
- Database: Microsoft SQL Server
- ORM: Entity Framework Core
- Authentication: JWT (JSON Web Tokens)
- Security: BCrypt.Net-Next (Password Hashing), CORS Policy configured
- Data Mapping: AutoMapper
- Validation: FluentValidation

## Architecture & Design Patterns
- Generic Repository Pattern: Implemented for DRY, clean data access, allowing dynamic querying and separation of concerns.
- Global Query Filters: Used to implement the Soft Delete pattern, ensuring data integrity at the database level automatically.
- Pagination Metadata Wrapper: Utilizing a generic PagedResult<T> class to deliver seamless pagination metrics (Total Pages, Current Page, etc.) to the client.
- DTOs (Data Transfer Objects): Used to encapsulate data payloads and protect internal database models.
- Global Error Handling: Centralized exception management middleware to provide consistent and secure API responses across development and production environments.

## Key Features
- Advanced Data Querying: Highly optimized server-side pagination, search filtering, and sorting executing directly on SQL Server using IQueryable.
- Data Integrity (Soft Delete): Entities are securely hidden using EF Core filters rather than permanently deleted, preventing accidental data loss and relationship corruption.
- File Management: Secure processing of multipart/form-data for uploading and serving static files (Book Cover Images) locally.
- Authentication & Authorization: Secure user registration and login utilizing BCrypt for password hashing and JWT for route protection.
- Public & Private Routes: Read-only access for public users, while data modification requires a valid Bearer Token.

## Getting Started

### Prerequisites
- .NET SDK (Version 8.0 or later)
- SQL Server
- Visual Studio / Visual Studio Code or any preferred IDE

### Installation & Setup

1. Clone the repository:
   git clone [https://github.com/YourUsername/library-API.git](https://github.com/YourUsername/library-API.git)

2. Navigate to the project directory:
   cd library-API

3. Configuration:
   Open the appsettings.json file and update the following:
   - ConnectionStrings: Point it to your local SQL Server instance.
   - Jwt: Update the Key with a strong, secure secret string.

4. Database Migration:
   Apply the migrations to generate the database tables by running the following command in the Package Manager Console or Terminal:
   dotnet ef database update

5. Run the Application:
   dotnet run
   
   The API will start, and you can explore the endpoints using tools like Postman or the built-in Swagger UI interface.

## API Endpoints Summary

### Auth
- POST /api/auth/register - Create a new user account.
- POST /api/auth/login - Authenticate user and generate JWT.

### Books
- GET /api/books?PageNumber=1&PageSize=10&SearchTerm=keyword&OrderBy=title - Retrieve a paginated, filtered, and sorted list of books.
- POST /api/books - Add a new book with cover image upload (multipart/form-data) (Requires Token).
- PUT /api/books/{id} - Update book details and optionally replace the cover image (Requires Token).
- DELETE /api/books/{id} - Soft delete a book and remove its physical image file (Requires Token).

### Authors
- GET /api/authors - Retrieve the list of all authors.
- POST /api/authors - Add a new author (Requires Token).
- PUT /api/authors/{id} - Update author details (Requires Token).
- DELETE /api/authors/{id} - Soft delete an author (Requires Token).
