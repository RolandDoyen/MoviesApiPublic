# Movies API (ASP.NET 8)
Une API RESTful robuste pour la gestion d'un catalogue de films, propulsée par **.NET 8**, **Entity Framework Core**, et **AutoMapper**. Elle propose un cycle de vie CRUD complet et une interface Swagger pour une documentation interactive.

> **Note :** Ce dépôt public est une version finalisée du projet destinée à mon portfolio. Le développement et les pipelines CI/CD vers Azure sont gérés via un dépôt privé, ce qui explique l'historique simplifié des commits ici.


## 📌 Table des matières
- [Movies API (ASP.NET 8)](#movies-api-aspnet-8)
  - [📌 Table des matières](#-table-des-matières)
  - [🚀 Démo en direct (Swagger)](#-démo-en-direct-swagger)
  - [🛠️ Stack Technique](#️-stack-technique)
  - [🏛️ Architecture \& Philosophie](#️-architecture--philosophie)
  - [📂 Structure du projet](#-structure-du-projet)
  - [📡 Points d'entrée de l'API (Endpoints)](#-points-dentrée-de-lapi-endpoints)
  - [🔄 Politique de Versionnage de l'API](#-politique-de-versionnage-de-lapi)
  - [🔐 Authentification \& JWT](#-authentification--jwt)
  - [⚠️ Gestion des erreurs](#️-gestion-des-erreurs)
  - [⚙️ Installation \& Configuration locale](#️-installation--configuration-locale)


## 🚀 Démo en direct (Swagger)
L'API est déployée sur Azure, et la documentation interactive est disponible via l'interface Swagger UI :  
**[👉 Explorer Swagger UI](https://moviesapi-rd.azurewebsites.net/swagger/index.html)**


## 🛠️ Stack Technique
- **Framework :** .NET 8 (ASP.NET Core Web API) pour la haute performance et la scalabilité.
- **ORM & Données :** Entity Framework Core 8 avec SQL Server pour une persistance fluide des données.
- **Mapping :** AutoMapper pour une séparation stricte entre les entités de la base de données et les DTOs.
- **Documentation :** Swagger (Swashbuckle) pour des spécifications OpenAPI standardisées.
- **Sécurité :** Authentification JWT et validation des données via DataAnnotations.
- **DevOps :** GitHub Actions pour le CI/CD automatisé vers Azure App Service.


## 🏛️ Architecture & Philosophie
L'API adopte une **Architecture en Couches** pour garantir une séparation claire des responsabilités :

```html
  Controller (API)
         ↕ AutoMapper
  BLL (Business Logic Layer)
         ↕ DataContext / DAO
  DAL (Data Access Layer)
         ↓
  Database (SQL Server)
```

- **Controller** : Expose les points d'entrée et mappe les requêtes/réponses.
- **BLL** : Contient la validation, les règles métier et le mapping DTO.
- **DAL** : Gère l'accès à la base de données et les entités EF Core.
- **Database** : SQL Server avec la table `Movies`.


## 📂 Structure du projet
La solution est organisée en plusieurs projets pour assurer une séparation stricte des responsabilités :

- **Movies.API** : La couche de présentation.
  - `Controllers/` : Points d'entrée de l'API (ex: `MovieController.cs`).
  - `Models/` : Modèles de requête et de réponse utilisés comme contrats.
  - `Profiles/` : Profils AutoMapper pour le mapping vers les DTOs.
  - `appsettings.json` : Configuration (chaînes de connexion, JWT, etc.).
  - `Program.cs` : Configuration des services et pipeline middleware.

- **Movies.BLL** : La couche de logique métier (Business Logic Layer).
  - `BLL/` : Implémentation des règles métier (ex: `MovieBLL.cs`).
  - `DTO/` : Objets de transfert de données pour la communication entre couches.
  - `Interfaces/` : Contrats de service (ex: `IMovieBLL.cs`).
  - `Profiles/` : Profils AutoMapper pour le mapping DTO vers Entité.

- **Movies.Core** : La couche partagée.
  - `Exceptions/` : Exceptions personnalisées (ex: `MovieNotFoundException.cs`).

- **Movies.DAL** : La couche d'accès aux données (Data Access Layer).
  - `DAO/` : Entités EF Core (ex: `Movie.cs`).
  - `Migrations/` : Historique des migrations de la base de données.
  - `DataContext.cs` : Contexte de base de données Entity Framework.


## 📡 Points d'entrée de l'API (Endpoints)
L'API suit les conventions RESTful pour la gestion des films. Tous les points d'entrée sont versionnés et renvoient des réponses JSON standardisées.

```markdown
| Method     | Endpoint             | Description                                       | Statut   | Auth Requise  |
| **GET**    | `/api/v2/token`      | Génère un token JWT pour la session               | Actif    | Non           |
| **GET**    | `/api/v1/movie`      | Récupérer tous les films du catalogue             | Obsolète | **Oui (JWT)** |
| **GET**    | `/api/v2/movie`      | Récupérer tous les films du catalogue             | Actif    | **Oui (JWT)** |
| **GET**    | `/api/v2/movie/{id}` | Récupérer les détails d'un film spécifique par ID | Actif    | **Oui (JWT)** |
| **POST**   | `/api/v2/movie`      | Créer une nouvelle entrée de film                 | Actif    | **Oui (JWT)** |
| **PUT**    | `/api/v2/movie/{id}` | Mettre à jour les détails d'un film existant      | Actif    | **Oui (JWT)** |
| **DELETE** | `/api/v2/movie/{id}` | Supprimer un film de la base de données           | Actif    | **Oui (JWT)** |
```

> **Note :** Les points d'entrée protégés nécessitent un jeton (token) Bearer valide inclus dans l'en-tête Authorization. Pour les besoins de la démonstration, le point d'accès /api/v2/token est ouvert publiquement, permettant de générer un jeton de test sans authentification préalable.


## 🔄 Politique de Versionnage de l'API
<summary>Ce projet utilise le Versionnage par URI pour garantir la rétrocompatibilité.</summary>
<summary>v2 (Actuelle) : Inclut des structures de données optimisées et les derniers correctifs de sécurité.</summary>
<summary>v1 (Obsolète) : Toujours active mais ne reçoit plus de mises à jour. Sera supprimée dans les futures versions.</summary>
<summary>Note d'implémentation : L'obsolescence est signalée dans les réponses de l'API via un en-tête HTTP personnalisé X-API-Deprecated.</summary>


## 🔐 Authentification & JWT
Pour sécuriser les opérations d'écriture et de suppression, cette API implémente l'authentification **JWT (JSON Web Token)**.

**Flux de travail :**
1. **Demande de jeton** : Accédez au point d'entrée `/token` pour générer un jeton Bearer.
2. **En-tête d'autorisation** : Pour les routes protégées incluez le jeton dans l'en-tête HTTP :
   `Authorization: Bearer <votre_jeton>`
3. **Configuration** :
   - L'émetteur (issuer), l'audience et la clé secrète du jeton sont configurés dans le fichier `appsettings.json`.
   - En production, ces paramètres doivent être gérés via des variables d'environnement ou Azure Key Vault.


## ⚠️ Gestion des erreurs
L'API implémente une stratégie de gestion centralisée des erreurs à l'aide d'exceptions personnalisées afin de garantir des réponses HTTP cohérentes et explicites.

**Exceptions personnalisées :**
- **`MovieNotFoundException`** : Renvoie un statut **404 Not Found** lorsqu'un ID demandé n'existe pas dans la base de données.
- **`MovieAlreadyExistsException`** : Renvoie un statut **409 Conflict** si une tentative est faite pour créer une entrée en double (ex: même Titre et Année).

**Exemple de réponse d'erreur :**
En cas d'erreur, l'API renvoie un objet structuré pour aider le consommateur à déboguer :

```json
{
  "status": 404,
  "message": "Le film avec l'ID 42 n'a pas été trouvé.",
  "timestamp": "2025-12-23T20:40:00Z"
}
```

**Erreurs de validation**
L'intégrité des données est renforcée par l'utilisation de **Data Annotations** sur les modèles DTO. Si une requête échoue à la validation (ex: champs obligatoires manquants, notes invalides ou chaînes dépassant la limite de caractères), l'API renvoie automatiquement un **400 Bad Request**.

**Règles de validation (Data Annotations) :**
- **Required** : Les champs essentiels comme le Titre et l'Année' ne peuvent pas être nuls.
- **Range** : Les notes doivent être comprises entre 0 et 10. L'Année doit être réaliste (entre 1930 et 2030).
- **StringLength** : Limites maximales de caractères pour les descriptions et les titres afin d'éviter les débordements de base de données.

**Exemple de réponse de validation :**
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


## ⚙️ Installation & Configuration locale
Suivez ces étapes pour obtenir une copie locale du projet et l'exécuter sur votre machine.

**Prérequis :**
* **SDK .NET 8**
* **SQL Server** (LocalDB ou Express)
* **Outils Entity Framework Core** (Installation via : `dotnet tool install --global dotnet-ef`)

**Étapes :**
1. **Cloner le dépôt :**
   ```bash
   git clone https://github.com/RolandDoyen/MoviesApiPublic.git
   ```

2. **Restaurer les packages NuGet :**
   ```bash
   dotnet restore
   ```

3. **Configurer la base de données :**
   Update the `DefaultConnection` string in `Movies.API/appsettings.json` to point to your local SQL Server instance.

4. **Exécuter les migrations :**
   Apply the database schema to your local instance:
   ```bash
   dotnet ef database update --project Movies.DAL --startup-project Movies.API
   ```

5. **Lancer l'API :**
   ```bash
   dotnet run --project Movies.API
   ```

Une fois l'application lancée, l'interface Swagger sera disponible à l'adresse `https://localhost:XXXX/swagger`.