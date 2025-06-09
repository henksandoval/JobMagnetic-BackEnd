# **JobMagnetic: Ecosistema Profesional Centralizado**

## 🚀 Resumen

JobMagnetic es una plataforma diseñada para revolucionar la forma en que los profesionales gestionan y presentan sus perfiles de carrera. Este repositorio contiene la aplicación backend en C# que impulsa todo el ecosistema. Proporciona una API RESTful robusta y escalable para la gestión de perfiles, el análisis de CVs mediante IA y todo el manejo de datos, construida con .NET y los principios de Arquitectura Limpia.

## ✨ Funcionalidades y Características Principales

Este backend habilita las siguientes características clave de la plataforma:

-   **Análisis de CV con IA:** Extrae de forma inteligente información clave de los archivos de CV proporcionados por los usuarios para construir y poblar perfiles profesionales automáticamente.
-   **Gestión Completa de Perfiles:** Expone operaciones CRUD (Crear, Leer, Actualizar, Eliminar) completas para todas las entidades del dominio.
-   **Generación de Perfiles Públicos:** Proporciona todos los endpoints de datos necesarios para que las aplicaciones frontend rendericen sitios profesionales públicos elegantes y optimizados.
-   **API Segura y Versionada:** Una API RESTful bien definida con versionado para garantizar interacciones estables y predecibles.

## 🛠️ Stack Tecnológico y Herramientas

**Stack Principal:**
-   C# & .NET 9.0
-   ASP.NET Core (para APIs RESTful)
-   Entity Framework Core (para acceso a datos y migraciones)
-   MS SQL Server

**Librerías de API y Framework:**
-   `Asp.Versioning.Mvc` (para el versionado de la API)
-   `FastAdapter.Adaptor` (para el mapeo de objetos de alto rendimiento)

**Desarrollo y Compilación:**
-   .NET SDK 9.0
-   JetBrains Rider / Visual Studio / VS Code

## 🏗️ Arquitectura

El proyecto se adhiere a un enfoque de **Arquitectura Limpia** (Clean Architecture), promoviendo una clara separación de responsabilidades que mejora la mantenibilidad, escalabilidad y la capacidad de realizar pruebas.

1.  **Capa de Dominio (`JobMagnet.Domain`):** Contiene la lógica de negocio principal: entidades (ej. `ProfileEntity`), objetos de valor e interfaces específicas del dominio (ej. `ICommandRepository`, `IUnitOfWork`). No tiene dependencias de otras capas del proyecto.

2.  **Capa de Aplicación (`JobMagnet.Application`):** Orquesta los casos de uso (funcionalidades) de la aplicación. Contiene la lógica de la aplicación, manejadores de comandos/consultas, DTOs, mapeadores e interfaces de servicio a nivel de aplicación. Depende de la capa de Dominio.

3.  **Capa de Infraestructura (`JobMagnet.Infrastructure`):** Proporciona implementaciones concretas para las interfaces definidas en las capas superiores. Esto incluye la lógica de persistencia de datos (EF Core `DbContext`, Repositorios) e integraciones con servicios externos (ej. `GeminiRawCvParser` para el análisis de CV).

4.  **Capa de Presentación (`JobMagnet.Host`):** Expone los endpoints de la API usando Controladores de ASP.NET Core. Maneja el ciclo de vida HTTP, la validación de solicitudes, la autenticación y la serialización. Es el punto de entrada de la aplicación.

5.  **Capa Compartida (`JobMagnet.Shared`):** Contiene utilidades, extensiones o clases base comunes que pueden ser utilizadas en múltiples capas o proyectos sin introducir dependencias no deseadas.

## 📁 Estructura del Proyecto

La solución está organizada en los siguientes proyectos clave, ubicados en los directorios `sources` y `tests` respectivamente:

-   `sources/JobMagnet.Domain/`: Entidades de negocio principales e interfaces de dominio.
-   `sources/JobMagnet.Application/`: Lógica de aplicación, casos de uso (comandos/consultas) y DTOs.
-   `sources/JobMagnet.Infrastructure/`: Acceso a datos (EF Core), implementación de repositorios y servicios de terceros.
-   `sources/JobMagnet.Host/`: Proyecto de la API ASP.NET Core, controladores y configuración de inicio.
-   `sources/JobMagnet.Shared/`: Utilidades y extensiones compartidas.
-   `tests/`: Contiene todos los proyectos de pruebas unitarias, de integración y compartidas.

## 🚀 Cómo Empezar

### Prerrequisitos

-   [.NET SDK 9.0]
-   Un IDE como [JetBrains Rider], [Visual Studio] o [VS Code].
-   Una instancia local o remota de MS SQL Server.
-   [Herramientas de dotnet-ef] instaladas globalmente.

### ⚙️ Instalación y Ejecución Local

1.  **Clonar el repositorio:**
    ```bash
    git clone https://github.com/henksandoval/JobMagnetic-BackEnd.git
    cd JobMagnetic-BackEnd
    ```

2.  **Restaurar los paquetes NuGet:**
    ```bash
    dotnet restore JobMagnet.sln
    ```

3.  **Configurar la Conexión a la Base de Datos:**
    *   En `sources/JobMagnet.Host/`, renombra `appsettings.Example.json` a `appsettings.Development.json`.
    *   Actualiza la cadena de conexión `DefaultConnection` con los detalles de tu base de datos.
    Ejemplo para LocalDB:
        ```json
        "ConnectionStrings": {
          "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=JobMagnetDB;Trusted_Connection=True;MultipleActiveResultSets=true"
        }
        ```

4.  **Aplicar las Migraciones de la Base de Datos:**
    > **Nota:** Debido a que nuestro `DbContext` reside en `Infrastructure` y nuestra configuración de inicio en `Host`, debes especificar ambos proyectos al ejecutar comandos de EF Core desde la raíz de la solución.

    ```bash
    # Crear una nueva migración
    dotnet ef migrations add InitialCreate --project sources/JobMagnet.Infrastructure --startup-project sources/JobMagnet.Host

    # Aplicar las migraciones a la base de datos
    dotnet ef database update --project sources/JobMagnet.Infrastructure --startup-project sources/JobMagnet.Host
    ```

5.  **Compilar el proyecto:**
    ```bash
    dotnet build JobMagnet.sln
    ```

### Ejecutar la Aplicación

1.  **Navegar al directorio del proyecto Host:**
    ```bash
    cd sources/JobMagnet.Host
    ```
2.  **Ejecutar la aplicación:**
    ```bash
    dotnet run
    ```
    Alternativamente, puedes ejecutar `JobMagnet.Host` directamente desde tu IDE.

La API será accesible típicamente en:
-   `https://localhost:7XXX` (HTTPS)
-   `http://localhost:5XXX` (HTTP)

Revisa la salida de la consola de `JobMagnet.Host` para ver los puertos exactos.

### Documentación de la API (Swagger/OpenAPI)

Una vez que la aplicación está en ejecución, la documentación interactiva de la API (Swagger UI) estará disponible en:
**`https://localhost:7109/swagger/index.html`**

### (Opcional) Poblar la Base de Datos con Datos de Ejemplo

Usa la interfaz de Swagger o los comandos `curl` a continuación para poblar la base de datos con datos iniciales.

```bash
# Para poblar las tablas maestras (ej. habilidades, idiomas)
curl -X POST 'https://localhost:7109/api/v0.1/admin/seedmastertables'

# Para poblar un perfil de ejemplo para pruebas
curl -X POST 'https://localhost:7109/api/v0.1/admin/seedprofile'
```

## 🧪 Ejecutar Pruebas y Generar Informes de Cobertura

Ejecuta estos comandos desde el directorio raíz de la solución.

1.  **Ejecutar todas las pruebas:**
    ```bash
    dotnet test JobMagnet.sln --no-build
    ```

2.  **Generar un informe de cobertura (HTML):**
    *   Primero, instala la herramienta generadora de informes:
        ```bash
        dotnet tool install --global dotnet-reportgenerator-globaltool
        ```
    *   Luego, ejecuta las pruebas y genera el informe en un solo paso:
        ```bash
        dotnet test JobMagnet.sln --collect:"XPlat Code Coverage"
        reportgenerator -reports:"**/TestResults/**/coverage.cobertura.xml" -targetdir:"coverage_report" -reporttypes:Html
        ```
    *Esto crea un informe HTML detallado en el directorio `coverage_report`.*

## 🤝 Cómo Contribuir

¡Las contribuciones son bienvenidas! Ya sea corrigiendo errores, añadiendo funcionalidades, mejorando la documentación o ampliando las pruebas, tu ayuda es apreciada. Siéntete libre de hacer un fork del repositorio y enviar un pull request.

## 📜 Licencia

Este proyecto está bajo la Licencia MIT - consulta el archivo [LICENSE.md](LICENSE.md) para más detalles.

<div align="center">
  <p>
	For the English version, <a href="README.md">click here</a>.
  </p>
</div>