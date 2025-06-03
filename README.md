# 🚀 JobMagnet: Your Professional Profile, Magnetized!

## Overview

JobMagnet is a platform designed to revolutionize how professionals manage and showcase their career profiles. It empowers users by automatically transforming their CVs into elegant, public-facing professional sites. The core idea is to connect professionals with opportunities—be it new job roles, collaborations, or interested visitors—by making their information easily accessible and well-presented.

This repository contains the C# backend application that powers JobMagnet. It provides a robust RESTful API for profile management, AI-driven CV parsing, and data handling, built with ASP.NET Core.

## ✨ Key Features

-   **AI-Powered CV Parsing:** Intelligently extracts key information from CV files (provided by users) to automatically build and populate professional profiles.
-   **Comprehensive Profile Management:** Full CRUD (Create, Read, Update, Delete) operations for job profiles.
-   **Public Profile Generation:** Transforms user data into an elegant and optimized public-facing professional site (handled by a potential frontend, API provides the data).
-   **RESTful API Endpoints:** A well-defined API for interacting with profile data and CV processing.
-   **Layered Architecture:** Follows clean architecture principles for separation of concerns, testability, and maintainability.
-   **Efficient Data Access:** Utilizes the Repository pattern for abstracting data persistence logic.
-   **Modern Object Mapping:** Employs FastAdapter.Adaptor for seamless mapping between Data Transfer Objects (DTOs) and domain entities.

## 🛠️ Technologies & Tools

**Core Stack:**
-   C# & .NET 9.0
-   ASP.NET Core (for RESTful APIs)
-   Entity Framework Core (for data access and migrations)

**API & Framework:**
-   `Asp.Versioning.Mvc` (for API versioning)
-   `FastAdapter.Adaptor` (for object mapping)

**Development & Build:**
-   .NET SDK 9.0
-   JetBrains Rider / Visual Studio / VS Code

## 🏗️ Architecture

The JobMagnet project adheres to a Clean Architecture approach, promoting a clear separation of concerns and enhancing maintainability and testability. The main logical layers are mapped to the following projects:

1.  **Domain Layer (`JobMagnet.Domain`):**
    *   Contains the core business logic: entities (e.g., `ProfileEntity`, `SkillEntity`), value objects, and domain-specific interfaces (e.g., `ICommandRepository`, `IQueryRepository`, `IUnitOfWork`).
    *   This layer has no dependencies on other project layers.

2.  **Application Layer (`JobMagnet.Application`):**
    *   Orchestrates the use cases (features) of the application.
    *   Contains application logic, command/query handlers (see `UseCases/CvParser`), DTOs (see `Contracts/Commands`, `Contracts/Responses`), mappers, and application-level service interfaces (e.g., `ICurrentUserService`, `IRawCvParser`).
    *   Depends on the Domain layer.

3.  **Infrastructure Layer (`JobMagnet.Infrastructure`):**
    *   Provides concrete implementations for interfaces defined in the Application and Domain layers.
    *   Includes data persistence logic (EF Core `JobMagnetDbContext`, repository implementations like `Repository.cs`, `UnitOfWork.cs`, and migrations).
    *   Handles external concerns like third-party API integrations (e.g., `GeminiRawCvParser.cs` for CV parsing via Gemini) and other infrastructure services.
    *   Depends on the Application and Domain layers.

4.  **Presentation Layer / Host (`JobMagnet.Host`):**
    *   Exposes the API endpoints using ASP.NET Core (Controllers).
    *   Handles HTTP requests/responses, authentication, authorization, request validation, and serialization.
    *   Depends on the Application layer to execute commands and queries.

5.  **Shared Layer (`JobMagnet.Shared`):**
    *   Contains common utilities, extensions, or base classes that can be used across multiple layers or projects without introducing unwanted dependencies. For example, `StringExtensions.cs`.

## 📁 Project Structure

The solution is organized into the following key projects, located under the `sources` and `tests` directories respectively:

**Source Projects (`sources/`):**
-   `JobMagnet.Domain/`: Core business entities, value objects, and domain interfaces.
-   `JobMagnet.Application/`: Application logic, use cases (commands/queries), DTOs, and application-level interfaces.
-   `JobMagnet.Infrastructure/`: Implementations for data access (EF Core, repositories), CV parsing services (Gemini), and other external concerns.
-   `JobMagnet.Host/`: ASP.NET Core API project, controllers, `Program.cs`, and API-specific configurations.
-   `JobMagnet.Shared/`: Common utilities and extensions shared across projects.

**Test Projects (`tests/`):**
-   `JobMagnet.Unit.Tests/`: Unit tests, primarily for mappers and other isolated components.
-   `JobMagnet.Integration.Tests/`: Integration tests, focusing on controller interactions, database operations, and external service mocks.
-   `JobMagnet.Shared.Tests/`: Tests for the utilities and extensions in the `JobMagnet.Shared` project.

## 🚀 Getting Started

### Prerequisites

-   [.NET SDK 9.0]
-   An IDE such as [JetBrains Rider], [Visual Studio], or [VS Code].
-   A local or remote MS SQL Server instance

### Installation & Setup

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
    *   Open `sources/JobMagnet.Host/appsettings.Development.json` (o `appsettings.json`).
    *   Update the `ConnectionStrings` section with your database connection details. Example for LocalDB:
        ```json
        "ConnectionStrings": {
          "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=JobMagnetDB;Trusted_Connection=True;MultipleActiveResultSets=true"
        }
        ```

4.  **Apply Database Migrations:**
    *   Ensure you have `dotnet-ef` tools installed globally. If not: `dotnet tool install --global dotnet-ef`
    *   **Important:** Due to the project's layered structure, where the `DbContext` is in `JobMagnet.Infrastructure` and the startup configurations are in `JobMagnet.Host`, you need to specify both projects when managing migrations.
    *   From the root directory of the solution (e.g., `C:\Repos\JobMagnetic-BackEnd`), run the following command to generate and apply migrations:
        ```bash
        dotnet ef migrations add MIGRATION_NAME --project .\sources\JobMagnet.Infrastructure\JobMagnet.Infrastructure.csproj --startup-project .\sources\JobMagnet.Host\JobMagnet.Host.csproj
        ```
        ```bash
        dotnet ef database update --project .\sources\JobMagnet.Infrastructure\JobMagnet.Infrastructure.csproj --startup-project .\sources\JobMagnet.Host\JobMagnet.Host.csproj
        ```
        This command tells Entity Framework Core to:
        - Use the migration files from the `JobMagnet.Infrastructure` project.
        - Use the `JobMagnet.Host` project to get the necessary configurations (like the database connection string) to apply these migrations.

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

Once the application is running, API documentation (Swagger UI) should be available at:
`https://localhost:7109/swagger/index.html`

## 🧪 Running Tests and Generating Coverage Report

The following commands should be run from the root directory of the solution.

1.  **Run all tests:**
    ```bash
    dotnet test JobMagnet.sln --no-build
    ```

2.  **Run tests with coverage and generate OpenCover report:**
    ```bash
    dotnet test JobMagnet.sln --no-build /p:CollectCoverage=true /p:CoverletOutput=./coverage.opencover.xml /p:CoverletOutputFormat=opencover
    ```
    *This will place `coverage.opencover.xml` in the solution root.*

3.  **Generate HTML coverage report (using ReportGenerator):**
    *   First, install the tool if you haven't:
        ```bash
        dotnet tool install --global dotnet-reportgenerator-globaltool
        ```
    *   Then, generate the report:
        ```bash
        reportgenerator -reports:./coverage.opencover.xml -targetdir:./coverage_report -reporttypes:Html
        ```
    *This will create an HTML report in the `coverage_report` directory in the solution root.*

## 🤝 Contributing

We welcome contributions to JobMagnet! Whether it's fixing bugs, adding features, improving documentation, or enhancing tests, your help is appreciated.


## 📜 License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details.