# **JobMagnetic: Centralized Professional Ecosystem**

## 🚀 Overview

JobMagnetic is a platform designed to revolutionize how professionals manage and showcase their career profiles. This repository contains the C# backend application that powers the entire ecosystem. It provides a robust and scalable RESTful API for profile management, AI-driven CV parsing, and all core data handling, built with .NET and Clean Architecture principles.

## ✨ Core Functionality & Features

This backend enables the following key platform features:

-   **AI-Powered CV Parsing:** Intelligently extracts key information from user-provided CV files to automatically build and populate professional profiles.
-   **Comprehensive Profile Management:** Exposes full CRUD (Create, Read, Update, Delete) operations for all domain entities.
-   **Public Profile Generation:** Provides all necessary data endpoints for front-end applications to render elegant and optimized public-facing professional sites.
-   **Secure & Versioned API:** A well-defined RESTful API with versioning to ensure stable and predictable interactions.

## 🛠️ Technology Stack & Tools

**Core Stack:**
-   C# & .NET 9.0
-   ASP.NET Core (for RESTful APIs)
-   Entity Framework Core (for data access and migrations)
-   MS SQL Server

**API & Framework Libraries:**
-   `Asp.Versioning.Mvc` (for API versioning)
-   `FastAdapter.Adaptor` (for high-performance object mapping)

**Development & Build:**
-   .NET SDK 9.0
-   JetBrains Rider / Visual Studio / VS Code

## 🏗️ Architecture

The project adheres to a **Clean Architecture** approach, promoting a clear separation of concerns which enhances maintainability, scalability, and testability.

1.  **Domain Layer (`JobMagnet.Domain`):** Contains the core business logic: entities (e.g., `ProfileEntity`), value objects, and domain-specific interfaces (e.g., `ICommandRepository`, `IUnitOfWork`). It has no dependencies on other layers.

2.  **Application Layer (`JobMagnet.Application`):** Orchestrates the application's use cases (features). Contains command/query handlers, DTOs, mappers, and application-level service interfaces. Depends on the Domain layer.

3.  **Infrastructure Layer (`JobMagnet.Infrastructure`):** Provides concrete implementations for interfaces defined in the layers above. This includes data persistence logic (EF Core `DbContext`, Repositories) and integrations with external services (e.g., `GeminiRawCvParser` for CV parsing).

4.  **Presentation Layer (`JobMagnet.Host`):** Exposes the API endpoints using ASP.NET Core Controllers. It handles the HTTP lifecycle, request validation, authentication, and serialization. This is the entry point of the application.

5.  **Shared Layer (`JobMagnet.Shared`):** Contains common utilities and extensions that can be used across multiple layers without introducing unwanted dependencies.

## 📁 Project Structure

The solution is organized into the following key projects, located under the `sources` and `tests` directories:

-   `sources/JobMagnet.Domain/`: Core business entities and domain interfaces.
-   `sources/JobMagnet.Application/`: Application logic, use cases (commands/queries), and DTOs.
-   `sources/JobMagnet.Infrastructure/`: Data access (EF Core), repository implementations, and third-party services.
-   `sources/JobMagnet.Host/`: ASP.NET Core API project, controllers, and startup configuration.
-   `sources/JobMagnet.Shared/`: Shared utilities and extensions.
-   `tests/`: Contains all unit, integration, and shared test projects.

## 🚀 Getting Started

### Prerequisites

-   [.NET SDK 9.0](https://dotnet.microsoft.com/download/dotnet/9.0)
-   An IDE like [JetBrains Rider](https://www.jetbrains.com/rider/), [Visual Studio](https://visualstudio.microsoft.com/), or [VS Code](https://code.visualstudio.com/).
-   A local or remote MS SQL Server instance.
-   [dotnet-ef tools](https://docs.microsoft.com/en-us/ef/core/cli/dotnet) installed globally.

### ⚙️ Setup and Local Execution

1.  **Clone the repository:**
    ```bash
    git clone https://github.com/henksandoval/JobMagnetic-BackEnd.git
    cd JobMagnetic-BackEnd
    ```

2.  **Restore NuGet packages:**
    ```bash
    dotnet restore JobMagnet.sln
    ```

3.  **Configure Database Connection:**
    *   In `sources/JobMagnet.Host/`, rename `appsettings.Example.json` to `appsettings.Development.json`.
    *   Update the `DefaultConnection` string with your database details.
    Example for LocalDB:
        ```json
        "ConnectionStrings": {
          "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=JobMagnetDB;Trusted_Connection=True;MultipleActiveResultSets=true"
        }
        ```

4.  **Apply Database Migrations:**
    > **Note:** Because our `DbContext` lives in `Infrastructure` and our configuration lives in `Host`, you must specify both projects when running EF Core commands from the solution root.

    ```bash
    # Create a new migration
    dotnet ef migrations add InitialDb --project sources/JobMagnet.Infrastructure --startup-project sources/JobMagnet.Host

    # Apply migrations to the database
    dotnet ef database update --project sources/JobMagnet.Infrastructure --startup-project sources/JobMagnet.Host
    ```

5.  **Build the project:**
    ```bash
    dotnet build JobMagnet.sln
    ```

6.  **(Optional) Populate the database with sample data:**
    *   To fill the database with sample data, send `POST` requests to the `AdminController` endpoints. You can use `curl` as shown below, or tools like Postman or the [Swagger UI](https://localhost:7109/swagger/index.html).

        Execute the following `curl` commands:
        ```bash
        # To seed master tables
        curl -X POST 'https://localhost:7109/api/v0.1/admin/seedmastertables' \
          -H 'accept: */*' \
          -d ''

        # To seed sample profiles
        curl -X POST 'https://localhost:7109/api/v0.1/admin/seedprofile' \
          -H 'accept: */*' \
          -d ''
        ```
    *   These requests will trigger the seeding logic defined within the `JobMagnet.Host` project. The Swagger UI is typically available at `/swagger` if your project is configured for it.

### Running the Application

1.  **Navigate to the Host project directory:**
    ```bash
    cd sources/JobMagnet.Host
    ```
2.  **Run the application:**
    ```bash
    dotnet run
    ```
    Alternatively, you can run `JobMagnet.Host` directly from your IDE.

The API will typically be accessible at:
-   `https://localhost:7XXX` (HTTPS)
-   `http://localhost:5XXX` (HTTP)

Check the console output from `JobMagnet.Host` for the exact ports.

### API Documentation (Swagger/OpenAPI)

Once the application is running, the interactive Swagger UI is available at:
**`https://localhost:7109/swagger/index.html`**

### (Optional) Seed the Database with Sample Data

Use the Swagger UI or the `curl` commands below to populate the database with initial data.

```bash
# To seed master tables (e.g., skills, languages)
curl -X POST 'https://localhost:7109/api/v0.1/admin/seedmastertables'

# To seed a sample profile for testing
curl -X POST 'https://localhost:7109/api/v0.1/admin/seedprofile'
```

## 🧪 Running Tests and Generating Coverage Reports

Run these commands from the root directory of the solution.

1.  **Run all tests:**
    ```bash
    dotnet test JobMagnet.sln --no-build
    ```

2.  **Generate a coverage report (HTML):**
    *   First, install the report generator tool:
        ```bash
        dotnet tool install --global dotnet-reportgenerator-globaltool
        ```
    *   Run tests and generate the report in one step:
        ```bash
        dotnet test JobMagnet.sln --collect:"XPlat Code Coverage"
        reportgenerator -reports:"**/TestResults/**/coverage.cobertura.xml" -targetdir:"coverage_report" -reporttypes:Html
        ```
    *This creates a detailed HTML report in the `coverage_report` directory.*

## 🤝 Contributing

We welcome contributions! Whether it's fixing bugs, adding features, or improving documentation, your help is appreciated. Please feel free to fork the repository and submit a pull request.

## 📜 License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details.

<div align="center">
  <p>
    For the Spanish version, <a href="README.es.md">click here</a>.
  </p>
</div>