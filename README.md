# Movies API (ASP.NET 8)
A robust RESTful API for managing a cinema catalog, powered by **.NET 8**, **Entity Framework Core**, and **AutoMapper**. It features a full CRUD lifecycle and a Swagger interface for interactive documentation.

> **Note:** This public repository is a polished version of the project for showcase purposes. Development and automated CI/CD pipelines to Azure are managed through a private repository, which explains the simplified commit history here.


## 📌 Table of Contents
- [Movies API (ASP.NET 8)](#movies-api-aspnet-8)
  - [📌 Table of Contents](#-table-of-contents)
  - [🚀 Live Demo (Swagger)](#-live-demo-swagger)
  - [🛠️ Tech Stack](#️-tech-stack)
  - [🏛️ Architecture \& Philosophy](#️-architecture--philosophy)
  - [📂 Project Structure](#-project-structure)
  - [📡 API Endpoints](#-api-endpoints)
  - [🔄 API Versioning Policy](#-api-versioning-policy)
  - [🔐 Authentication \& JWT](#-authentication--jwt)
  - [⚠️ Error Handling](#️-error-handling)
  - [⚙️ Installation \& Local Setup](#️-installation--local-setup)


## 🚀 Live Demo (Swagger)
The API is deployed on Azure, and the interactive documentation is available via the Swagger UI:  
**[👉 Explore Swagger UI](https://moviesapi-rd.azurewebsites.net/swagger/index.html)**


## 🛠️ Tech Stack
- **Framework:** .NET 8 (ASP.NET Core Web API) for high-performance and scalability.
- **ORM & Data:** Entity Framework Core 8 with SQL Server for seamless data persistence.
- **Mapping:** AutoMapper for strict separation between DB entities and DTOs.
- **Documentation:** Swagger (Swashbuckle) for standardized OpenAPI specifications.
- **Security:** JWT Authentication and data validation via DataAnnotations.
- **DevOps:** GitHub Actions for automated CI/CD to Azure App Service.


## 🏛️ Architecture & Philosophy
The API adopts a **Layered Architecture** to ensure a clear separation of concerns:

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


## 📂 Project Structure
The solution is organized into multiple projects to ensure a strict separation of concerns:

- **Movies.API**: The presentation layer.
  - `Controllers/`: API entry points (e.g., `MovieController.cs`).
  - `Models/`: Request and Response models used as contracts.
  - `Profiles/`: AutoMapper profiles for mapping to DTOs.
  - `appsettings.json`: Configuration (Database strings, JWT, etc.).
  - `Program.cs`: Service configuration and middleware pipeline.

- **Movies.BLL**: The Business Logic Layer.
  - `BLL/`: Implementation of core business rules (e.g., `MovieBLL.cs`).
  - `DTO/`: Data Transfer Objects for cross-layer communication.
  - `Interfaces/`: Service contracts (e.g., `IMovieBLL.cs`).
  - `Profiles/`: AutoMapper profiles for DTO to Entity mapping.

- **Movies.Core**: The shared layer.
  - `Exceptions/`: Custom exceptions (e.g., `MovieNotFoundException.cs`).

- **Movies.DAL**: The Data Access Layer.
  - `DAO/`: EF Core entities (e.g., `Movie.cs`).
  - `Migrations/`: Database migration history.
  - `DataContext.cs`: Entity Framework database context.


## 📡 API Endpoints
The API follows RESTful conventions for movie management. All endpoints are versioned and return standardized JSON responses.

```markdown
| Method     | Endpoint             | Description                           | Status     | Auth Required |
| **GET**    | `/api/v2/token`      | Generate a JWT token for the session  | Active     | No            |
| **GET**    | `/api/v1/movie`      | Retrieve all movies in the catalog    | Deprecated | **Yes (JWT)** |
| **GET**    | `/api/v2/movie`      | Retrieve all movies in the catalog    | Active     | **Yes (JWT)** |
| **GET**    | `/api/v2/movie/{id}` | Retrieve specific movie details by ID | Active     | **Yes (JWT)** |
| **POST**   | `/api/v2/movie`      | Create a new movie entry              | Active     | **Yes (JWT)** |
| **PUT**    | `/api/v2/movie/{id}` | Update an existing movie's details    | Active     | **Yes (JWT)** |
| **DELETE** | `/api/v2/movie/{id}` | Remove a movie from the database      | Active     | **Yes (JWT)** |
```
> **Note:** Protected endpoints require a valid Bearer Token to be included in the Authorization header. For demonstration purposes, the /api/v2/token endpoint is publicly accessible, allowing you to generate a test token without prior authentication.


## 🔄 API Versioning Policy
<summary>This project implements URI Versioning to ensure backward compatibility.</summary>
<summary>v2 (Current): Includes optimized data structures and the latest security patches.</summary>
<summary>v1 (Deprecated): Still active but no longer receiving updates. It will be decommissioned in future releases.</summary>
<summary>Implementation note: Deprecation is signaled in the API responses via a custom X-API-Deprecated HTTP header.</summary>


## 🔐 Authentication & JWT
To secure the write and delete operations, this API implements **JWT (JSON Web Token)** authentication.

**Workflow**
1. **Requesting a Token**: Access the `/token` endpoint to generate a Bearer token.
2. **Authorization Header**: For protected routes include the token in the HTTP header:
   `Authorization: Bearer <your_token>`
3. **Configuration**:
   - The token issuer, audience, and secret key are configured in `appsettings.json`.
   - In production, these should be managed via environment variables or Azure Key Vault.


## ⚠️ Error Handling
The API implements a centralized error handling strategy using custom exceptions to ensure consistent and meaningful HTTP responses.

**Custom Exceptions**
- **`MovieNotFoundException`**: Returns a **404 Not Found** status when a requested ID does not exist in the database.
- **`MovieAlreadyExistsException`**: Returns a **409 Conflict** status if an attempt is made to create a duplicate entry (e.g., same Title and Year).

**Response Format**
In case of an error, the API returns a structured object to help the consumer debug:

```json
{
  "status": 404,
  "message": "The movie with ID 42 was not found.",
  "timestamp": "2025-12-23T20:40:00Z"
}
```

**Validation Errors**
Data integrity is enforced using **Data Annotations** on the DTO models. If a request fails validation (e.g., missing required fields, invalid ratings, or strings exceeding character limits), the API automatically returns a **400 Bad Request**.

The validation rules include:
- **Required**: Essential fields like Title and Year cannot be null.
- **Range**: Ratings must be between 0 and 10. Year must be realistic (between 1930 and 2030).
- **StringLength**: Maximum character limits for descriptions and titles to prevent database overflow.

**Example Validation Response:**
```json
{
  "type": "https://tools.ietf.org/html/rfc9110#section-15.5.1",
  "title": "One or more validation errors occurred.",
  "status": 400,
  "errors": {
    "Year": [
      "Year must be realistic"
    ]
  },
  "traceId": "00-7d6598aae1e2c1adc4e3bea7ca94ae80-beffdf2a1aa703ac-00"
}
```


## ⚙️ Installation & Local Setup
Follow these steps to get a local copy of the project up and running on your machine.

**Prerequisites**
* **.NET 8 SDK**
* **SQL Server** (LocalDB or Express)
* **Entity Framework Core Tools** (Install via: `dotnet tool install --global dotnet-ef`)

**Steps** 
1. **Clone the repository:**
   ```bash
   git clone https://github.com/RolandDoyen/MoviesApiPublic.git
   ```

2. **Restore NuGet packages:**
   ```bash
   dotnet restore
   ```

3. **Configure the Database:**
   Update the `DefaultConnection` string in `Movies.API/appsettings.json` to point to your local SQL Server instance.

4. **Run Migrations:**
   Apply the database schema to your local instance:
   ```bash
   dotnet ef database update --project Movies.DAL --startup-project Movies.API
   ```

5. **Launch the API:**
   ```bash
   dotnet run --project Movies.API
   ```

Once the application is running, the Swagger UI will be available at `https://localhost:XXXX/swagger`.