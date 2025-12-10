## Movies API
Une API RESTful pour gérer des films, construite avec **.NET 8**, **Entity Framework Core** et **AutoMapper**. L'API fournit toutes les opérations CRUD et intègre Swagger pour une documentation automatique.

## Table des matières
- [Movies API](#movies-api)
- [Table des matières](#table-des-matières)
- [Aperçu du projet](#aperçu-du-projet)
- [Technologies](#technologies)
- [Architecture](#architecture)
- [Installation et exécution](#installation-et-exécution)
- [Structure du projet](#structure-du-projet)
- [Endpoints de l'API](#endpoints-de-lapi)
- [Modèles et DTO](#modèles-et-dto)
- [Gestion des erreurs](#gestion-des-erreurs)
- [Documentation Swagger](#documentation-swagger)

## Aperçu du projet
L'API Movies vous permet de :
- Créer un nouveau film  
- Récupérer tous les films  
- Récupérer un film par son ID  
- Mettre à jour un film existant  
- Supprimer un film  

Elle utilise une **architecture en couches** avec une séparation entre :
- **Controllers** : gèrent les requêtes et réponses HTTP  
- **BLL (Business Logic Layer / Couche métier)** : contient la logique principale pour les opérations CRUD  
- **DAL (Data Access Layer / Couche d'accès aux données)** : interagit avec la base de données via EF Core  
- **DTOs / Models** : font le mapping des entités vers les modèles de requête/réponse de l'API  

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

- **Controller** : expose les endpoints et mappe les requêtes/réponses  
- **BLL** : contient la validation, les règles métier et le mapping des DTO  
- **DAL** : gère l'accès à la base de données et les entités EF Core  
- **Database** : SQL Server avec la table `Movies`  

## Installation et exécution
1. Cloner le repository :
    ```html
    git clone https://github.com/RolandDoyen/ClanManagement
    ```
2. Restaurer les packages NuGet :
    ```html
    dotnet restore
    ```
3. Configurer la base de données dans appsettings.json :
    ```html
    "ConnectionStrings": {
    "DefaultConnection": "Server=SERVERNAME;Database=MoviesDB;Trusted_Connection=True;"
    }
    ```
4. Appliquer les migrations :
    ```html
    dotnet ef database update
    ```
5. Lancer l’application :
    ```html
    dotnet run --project Movies.API
    ```

## Structure du projet
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

## Endpoints de l'API
| Méthode | Endpoint           | Description                 |
| ------- | ------------------ | --------------------------- |
| POST    | /api/v1/movie      | Créer un nouveau film       |
| GET     | /api/v1/movie      | Récupérer tous les films    |
| GET     | /api/v1/movie/{id} | Récupérer un film par ID    |
| PUT     | /api/v1/movie/{id} | Mettre à jour un film par ID|
| DELETE  | /api/v1/movie/{id} | Supprimer un film par ID    |

## Modèles et DTO
- MovieDTO : objet de transfert de données utilisé dans la BLL  
- MovieRequestModel : modèle de requête pour créer/metre à jour des films  
- MovieResponseModel : modèle de réponse renvoyé par l'API  
- Movie : entité EF Core stockée dans la base de données  
- Toutes les propriétés sont documentées avec des commentaires XML pour activer Swagger et IntelliSense.  

## Gestion des erreurs
- MovieNotFoundException : levée lorsqu’un film n’est pas trouvé  
- MovieAlreadyExistsException : levée lors de la création d’un film qui existe déjà  
- Les controllers retournent les codes HTTP appropriés :
  - 200 OK en cas de succès
  - 404 Not Found si le film n’existe pas
  - 409 Conflict si le film existe déjà
  - 500 Internal Server Error pour les exceptions non gérées

## Documentation Swagger
- Activée via Swashbuckle.AspNetCore  
- Accessible à l’adresse https://localhost:<port>/swagger  
- Affiche tous les endpoints, les modèles de requête/réponse, ainsi que les commentaires XML  