using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net.Mime;
using System.Text;
using AutoFixture;
using FluentAssertions;
using JobMagnet.Application.Contracts.Commands.Profile;
using JobMagnet.Application.Contracts.Queries.Profile;
using JobMagnet.Application.Contracts.Responses.Profile;
using JobMagnet.Domain.Core.Entities;
using JobMagnet.Domain.Ports.Repositories;
using JobMagnet.Domain.Ports.Repositories.Base;
using JobMagnet.Host.ViewModels.Profile;
using JobMagnet.Integration.Tests.Fixtures;
using JobMagnet.Shared.Tests.Fixtures;
using JobMagnet.Shared.Tests.Fixtures.Builders;
using JobMagnet.Shared.Tests.Utils;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit.Abstractions;

namespace JobMagnet.Integration.Tests.Tests.Controllers.V1;

public class ProfileControllerShould : IClassFixture<JobMagnetTestSetupFixture>
{
    private const string RequestUriController = "api/v1/profile";
    private const string InvalidId = "100";
    private const int ContactInfoCount = 3;
    private const int TalentsCount = 8;
    private const int PortfolioCount = 3;
    private const int SummariesCount = 3;
    private const int TestimonialsCount = 12;
    private readonly IFixture _fixture = FixtureBuilder.Build();
    private readonly HttpClient _httpClient;
    private readonly JobMagnetTestSetupFixture _testFixture;

    public ProfileControllerShould(JobMagnetTestSetupFixture testFixture, ITestOutputHelper testOutputHelper)
    {
        _testFixture = testFixture;
        _httpClient = _testFixture.GetClient();
        _testFixture.SetTestOutputHelper(testOutputHelper);
    }

    [Fact(DisplayName = "Return the record and return 200 when GET request with valid Name is provided")]
    public async Task ReturnRecord_WhenValidNameProvidedAsync()
    {
        // Given
        var entity = await SetupEntityAsync();
        var queryParameters = new Dictionary<string, string>
        {
            { nameof(ProfileQueryParameters.Name), entity.FirstName }
        };

        var requestUrl = QueryHelpers.AddQueryString(RequestUriController, queryParameters!);

        // When
        var response = await _httpClient.GetAsync(requestUrl);

        // Then
        response.IsSuccessStatusCode.ShouldBeTrue();
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var responseData = await TestUtilities.DeserializeResponseAsync<ProfileViewModel>(response);
        responseData.ShouldNotBeNull();
        responseData.ShouldBeAssignableTo<ProfileViewModel>();

        responseData.PersonalData.ShouldNotBeNull();
        responseData.PersonalData.SocialNetworks.Length.ShouldBe(ContactInfoCount);
        responseData.About.ShouldNotBeNull();
        responseData.Testimonials!.Length.ShouldBe(TestimonialsCount);
        responseData.SkillSet.ShouldNotBeNull();
        responseData.PortfolioGallery!.Length.ShouldBe(PortfolioCount);
    }

    [Fact(DisplayName = "Create a new record and return 201 when the POST request is valid")]
    public async Task ReturnCreatedAndPersistData_WhenRequestIsValidAsync()
    {
        // Given
        await _testFixture.ResetDatabaseAsync();
        var createRequest = _fixture.Build<ProfileCommand>().Create();
        var httpContent = TestUtilities.SerializeRequestContent(createRequest);

        // When
        var response = await _httpClient.PostAsync(RequestUriController, httpContent);

        // Then
        response.IsSuccessStatusCode.ShouldBeTrue();
        response.StatusCode.ShouldBe(HttpStatusCode.Created);

        var responseData = await TestUtilities.DeserializeResponseAsync<ProfileResponse>(response);
        responseData.ShouldNotBeNull();

        var locationHeader = response.Headers.Location!.ToString();
        locationHeader.ShouldNotBeNull();
        locationHeader.ShouldContain($"{RequestUriController}/{responseData.Id}");

        await using var scope = _testFixture.GetProvider().CreateAsyncScope();
        var queryRepository = scope.ServiceProvider.GetRequiredService<IProfileQueryRepository>();
        var entityCreated = await queryRepository.GetByIdAsync(responseData.Id);

        entityCreated.ShouldNotBeNull();
        entityCreated.Should().BeEquivalentTo(createRequest, options => options.ExcludingMissingMembers());
    }

    [Fact(DisplayName = "Return the record and return 200 when GET request with valid ID is provided")]
    public async Task ReturnRecord_WhenValidIdIsProvidedAsync()
    {
        // Given
        var entity = await SetupEntityAsync();

        // When
        var response = await _httpClient.GetAsync($"{RequestUriController}/{entity.Id}");

        // Then
        response.IsSuccessStatusCode.ShouldBeTrue();
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var responseData = await TestUtilities.DeserializeResponseAsync<ProfileResponse>(response);
        responseData.ShouldNotBeNull();
        responseData.Should().BeEquivalentTo(entity, options => options.ExcludingMissingMembers());
    }

    [Fact(DisplayName = "Return 404 when GET request with invalid ID is provided")]
    public async Task ReturnNotFound_WhenInvalidIdIsProvidedAsync()
    {
        // Given
        _ = await SetupEntityAsync();

        // When
        var response = await _httpClient.GetAsync($"{RequestUriController}/{InvalidId}");

        // Then
        response.IsSuccessStatusCode.ShouldBeFalse();
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    [Fact(DisplayName = "Return 204 when a valid PUT request is provided")]
    public async Task ReturnNotContent_WhenReceivedValidPutRequestAsync()
    {
        // Given
        var entity = await SetupEntityAsync();
        var updateRequest = _fixture.Build<ProfileCommand>()
            .Create();

        // When
        var response = await _httpClient.PutAsJsonAsync($"{RequestUriController}/{entity.Id}", updateRequest);

        // Then
        response.IsSuccessStatusCode.ShouldBeTrue();
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);

        await using var scope = _testFixture.GetProvider().CreateAsyncScope();
        var queryRepository = scope.ServiceProvider.GetRequiredService<IProfileQueryRepository>();
        var dbEntity = await queryRepository.GetByIdAsync(entity.Id);
        dbEntity.ShouldNotBeNull();
        dbEntity.Should().BeEquivalentTo(updateRequest, options => options.ExcludingMissingMembers());
    }

    [Fact(DisplayName = "Return 404 when a PUT request with invalid ID is provided")]
    public async Task ReturnNotFound_WhenPutRequestWithInvalidIdIsProvidedAsync()
    {
        // Given
        await _testFixture.ResetDatabaseAsync();
        var updatedEntity = _fixture.Build<ProfileCommand>().Create();

        // When
        var response = await _httpClient.PutAsJsonAsync($"{RequestUriController}/{InvalidId}", updatedEntity);

        // Then
        response.IsSuccessStatusCode.ShouldBeFalse();
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    [Fact(DisplayName = "Create a new profile and return 201 when a valid CV file is loaded")]
    public async Task ReturnCreatedAndPersistData_WhenIsValidCVFileAsync()
    {
        // Given
        await _testFixture.ResetDatabaseAsync();

        const string fileName = "cv_laura_gomez.pdf";
        const string cvContent = """
                                 Laura Gómez Fernández
                                 (+34) 611-22-33-44 | laura.gomez.dev@example.net | linkedin.com/in/laura-gomez-fernandez | https://lauragomez.dev

                                 Resumen Profesional
                                 Ingeniera de Software con 7 años de experiencia en el desarrollo de aplicaciones web robustas y escalables. Especialista en el ecosistema .NET y Angular, con un historial probado en la entrega de proyectos de alta calidad, desde la concepción hasta el despliegue. Busco aplicar mis habilidades en arquitecturas de microservicios y soluciones en la nube para contribuir a proyectos innovadores.

                                 Educación
                                 Septiembre 2014 - Junio 2018
                                 Máster en Ingeniería de Software y Sistemas Informáticos
                                 Universidad Ficticia de Barcelona (UFB)

                                 Octubre 2010 - Julio 2014
                                 Grado en Ingeniería Informática
                                 Escuela Técnica Superior de Ingenieros Ficticia (ETSIF)

                                 Experiencia laboral
                                 Sr. Software Engineer .NET
                                 TechSolutions Global
                                 Marzo 2020 - Actualmente
                                 Diseño y desarrollo de componentes backend para una plataforma de e-commerce utilizando .NET Core, microservicios y Azure.
                                 Implementación de pipelines CI/CD con Azure DevOps para automatizar pruebas y despliegues.
                                 Colaboración en la definición de la arquitectura de nuevas funcionalidades y optimización de módulos existentes.
                                 Mentoría de desarrolladores junior y participación activa en revisiones de código.
                                 Tecnologias : (.NET Core, C#, ASP.NET Core, Microservices, Azure Functions, Azure Service Bus, Docker, Kubernetes, SQL Server, Cosmos DB, Git, Azure DevOps)

                                 Full Stack Developer
                                 Innovatech Digital
                                 Agosto 2018 - Febrero 2020
                                 Desarrollo full-stack de aplicaciones web para clientes del sector financiero y de seguros.
                                 Frontend desarrollado con Angular y TypeScript, consumiendo APIs RESTful construidas con ASP.NET Web API.
                                 Mantenimiento y evolución de sistemas legados, proponiendo mejoras y refactorizaciones.
                                 Participación en la planificación de sprints y ceremonias ágiles (Scrum).
                                 Tecnologias : (.NET Framework, ASP.NET MVC, Web API, C#, Entity Framework, Angular, TypeScript, JavaScript, HTML5, CSS3, SQL Server, JIRA)

                                 Certificaciones
                                 Microsoft Certified: Azure Developer Associate
                                 Microsoft - 2022 - Link_Azure_Cert
                                 Professional Scrum Master I (PSM I)
                                 Scrum.org - 2021 - Link_PSM_Cert
                                 Developing ASP.NET Core MVC Web Applications
                                 Pluralsight - 2020 - Link_Pluralsight_Course

                                 Habilidades Técnicas
                                 Lenguajes: C#, TypeScript, JavaScript, SQL
                                 Frameworks/Plataformas: .NET (Core, Framework), ASP.NET (Core, MVC, Web API), Entity Framework, Angular
                                 Bases de Datos: SQL Server, Cosmos DB, MongoDB (básico)
                                 Cloud: Azure (App Services, Functions, Service Bus, Kubernetes Service, DevOps)
                                 Herramientas: Docker, Kubernetes, Git, Visual Studio, VS Code, JIRA
                                 Otros: Microservicios, RESTful APIs, CI/CD, Metodologías Ágiles (Scrum, Kanban), DDD (conceptual)
                                 """;
        var fileBytes = Encoding.UTF8.GetBytes(cvContent);
        var memoryStream = new MemoryStream(fileBytes);

        using var multipartContent = new MultipartFormDataContent();
        var fileStreamContent = new StreamContent(memoryStream);
        fileStreamContent.Headers.ContentType = new MediaTypeHeaderValue(MediaTypeNames.Text.Plain);

        multipartContent.Add(fileStreamContent, "cvFile", fileName);

        // When
        var response = await _httpClient.PostAsync(RequestUriController + "/create-from-cv", multipartContent);

        // Then
        response.EnsureSuccessStatusCode();
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
    }

    private async Task<ProfileEntity> SetupEntityAsync()
    {
        await _testFixture.ResetDatabaseAsync();
        return await CreateAndPersistEntityAsync();
    }

    private async Task<ProfileEntity> CreateAndPersistEntityAsync()
    {
        await using var scope = _testFixture.GetProvider().CreateAsyncScope();
        var commandRepository = scope.ServiceProvider.GetRequiredService<ICommandRepository<ProfileEntity>>();

        var entity = new ProfileEntityBuilder(_fixture)
            .WithResume()
            .WithContactInfo(ContactInfoCount)
            .WithTalents(TalentsCount)
            .WithPortfolio(PortfolioCount)
            .WithSummaries(SummariesCount)
            .WithServices()
            .WithSkills()
            .WithTestimonials(TestimonialsCount)
            .Build();

        await commandRepository.CreateAsync(entity);

        return entity;
    }
}