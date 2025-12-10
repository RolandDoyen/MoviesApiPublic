## Movies API
A RESTful API for managing movies, built with **.NET 8**, **Entity Framework Core**, and **AutoMapper**. The API provides full CRUD operations and integrates Swagger for automatic documentation.

## Table of Contents
- [Movies API](#movies-api)
- [Table of Contents](#table-of-contents)
- [Project Overview](#project-overview)
- [Technologies](#technologies)
- [Architecture](#architecture)
- [Installation and Execution](#installation-and-execution)
- [Project Structure](#project-structure)
- [API Endpoints](#api-endpoints)
- [Models and DTOs](#models-and-dtos)
- [Error Handling](#error-handling)
- [Swagger Documentation](#swagger-documentation)
- [Authentication \& JWT Token](#authentication--jwt-token)

## Project Overview
The Movies API allows you to:
- Create a new movie  
- Retrieve all movies  
- Retrieve a movie by ID  
- Update an existing movie  
- Delete a movie  

It uses a **layered architecture** with separation between:
- **Controllers**: handle HTTP requests and responses  
- **BLL (Business Logic Layer)**: contains the core logic for CRUD operations  
- **DAL (Data Access Layer)**: interacts with the database via EF Core  
- **DTOs / Models**: map entities to API request/response models  

## Technologies
- .NET 8  
- ASP.NET Core Web API  
- Entity Framework Core 8  
- AutoMapper 16  
- Swashbuckle.AspNetCore 6 (Swagger)  
- SQL Server

## Architecture
  ```html
  Controller (API)
        ↕ AutoMapper
  BLL (Business Logic Layer)
        ↕ DataContext / DAO
  DAL (Data Access Layer)
        ↓
  Database (SQL Server)
  ```

- **Controller**: exposes endpoints and maps requests/responses  
- **BLL**: contains validation, business rules, and DTO mapping  
- **DAL**: manages database access and EF Core entities  
- **Database**: SQL Server with `Movies` table  

## Installation and Execution
1. Clone the repository:
    ```html
    git clone https://github.com/RolandDoyen/ClanManagement
    ```
2. Restore NuGet packages :
    ```html
    dotnet restore
    ```
3. Configure your database connection in appsettings.json:
    ```html
    "ConnectionStrings": {
    "DefaultConnection": "Server=SERVERNAME;Database=MoviesDB;Trusted_Connection=True;"
    }
    ```
4. Run migrations and update the database :
    ```html
    dotnet ef database update
    ```
5. Run the application :
    ```html
    dotnet run --project Movies.API
    ```

## Project Structure
- Movies.API
  - Controllers
    - MovieController.cs
  - Models
    - MovieRequestModel.cs
    - MovieResponseModel.cs
  - Profiles
    - MovieAPIProfile.cs
  - appsettings.json
  - Program.cs
- Movies.BLL
  - BLL
    - MovieBLL.cs
  - DTO
    - MovieDTO.cs
  - Interfaces
    - IMovieBLL.cs
  - Profiles
    - MovieBLLProfile.cs
- Movies.Core
  - Exceptions
    - MovieAlreadyExistsException.cs
    - MovieNotFoundException.cs
- Movies.DAL
  - DAO
    - Movie.cs
  - Migrations
  - DataContext.cs

## API Endpoints
| Method | Endpoint           | Description            |
| ------ | ------------------ | ---------------------- |
| POST   | /api/v1/movie      | Create a new movie     |
| GET    | /api/v1/movie      | Retrieve all movies    |
| GET    | /api/v1/movie/{id} | Retrieve a movie by ID |
| PUT    | /api/v1/movie/{id} | Update a movie by ID   |
| DELETE | /api/v1/movie/{id} | Delete a movie by ID   |

## Models and DTOs
- MovieDTO: Data transfer object used in BLL
- MovieRequestModel: Request model for creating/updating movies
- MovieResponseModel: Response model returned by the API
- Movie: EF Core entity stored in the database
- All properties are documented with XML comments to enable Swagger and IntelliSense.

## Error Handling
- MovieNotFoundException: thrown when a movie is not found
- MovieAlreadyExistsException: thrown when creating a movie that already exists
- Controllers return proper HTTP status codes:
  - 200 OK on success
  - 404 Not Found if movie does not exist
  - 409 Conflict if movie already exists
  - 500 Internal Server Error for unhandled exceptions

## Swagger Documentation
- Enabled via Swashbuckle.AspNetCore
- Access at https://localhost:<port>/swagger
- Shows all endpoints, request/response models, and XML comments

## Authentication & JWT Token
This API uses JWT (JSON Web Token) authentication for protected endpoints.
1. How to obtain a token
  - Send a request to :
    ```html
    POST /Token/GetToken
    ```
  - This will return a valid JWT signed using the secret configured in appsettings.json under :
    ```html
    "Jwt": {
      "Secret": "YOUR_LONG_SECURE_SECRET_KEY"
    }
    ```
The same secret must be used in your Program.cs for the JWT middleware and in the TokenController when generating the token.
The token expires after 24 hours.

2. Using the token in Swagger
   1. Open Swagger UI
   2. Click the Authorize button
   3. Enter the token
   4. You can now call all protected endpoints.