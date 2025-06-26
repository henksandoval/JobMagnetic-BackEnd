using System.Net;
using System.Net.Http.Json;
using AutoFixture;
using FluentAssertions;
using JobMagnet.Application.Contracts.Commands.Portfolio;
using JobMagnet.Application.Contracts.Responses.Base;
using JobMagnet.Application.Contracts.Responses.Portfolio;
using JobMagnet.Domain.Aggregates.Profiles;
using JobMagnet.Domain.Aggregates.Profiles.Entities;
using JobMagnet.Domain.Ports.Repositories.Base;
using JobMagnet.Integration.Tests.Fixtures;
using JobMagnet.Shared.Tests.Fixtures;
using JobMagnet.Shared.Tests.Fixtures.Builders;
using JobMagnet.Shared.Tests.Utils;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit.Abstractions;

namespace JobMagnet.Integration.Tests.Tests.Controllers.V1;

public class ProjectControllerShould : IClassFixture<JobMagnetTestSetupFixture>
{
    private const string RequestUriController = "api/v1/project";
    private const string InvalidId = "100";
    private readonly IFixture _fixture = FixtureBuilder.Build();
    private readonly HttpClient _httpClient;
    private readonly JobMagnetTestSetupFixture _testFixture;

    public ProjectControllerShould(JobMagnetTestSetupFixture testFixture, ITestOutputHelper testOutputHelper)
    {
        _testFixture = testFixture;
        _httpClient = _testFixture.GetClient();
        _testFixture.SetTestOutputHelper(testOutputHelper);
    }

    [Fact(DisplayName = "Create a new record and return 201 when the POST request is valid")]
    public async Task ReturnCreatedAndPersistData_WhenRequestIsValidAsync()
    {
        // --- Given ---
        await _testFixture.ResetDatabaseAsync();
        var profileEntity = await SetupProfileEntityAsync();
        var createRequest = _fixture.Build<ProjectCommand>()
            .With(x => x.ProjectData, GetProjectData(profileEntity.Id.Value))
            .Create();
        var httpContent = TestUtilities.SerializeRequestContent(createRequest);

        // --- When ---
        var response = await _httpClient.PostAsync(RequestUriController, httpContent);

        // --- Then ---
        response.IsSuccessStatusCode.ShouldBeTrue();
        response.StatusCode.ShouldBe(HttpStatusCode.Created);

        var responseData = await TestUtilities.DeserializeResponseAsync<ProjectResponse>(response);
        responseData.ShouldNotBeNull();

        var locationHeader = response.Headers.Location!.ToString();
        locationHeader.ShouldNotBeNull();
        locationHeader.ShouldContain($"{RequestUriController}/{responseData.Id}");

        await using var scope = _testFixture.GetProvider().CreateAsyncScope();
        var queryRepository =
            scope.ServiceProvider.GetRequiredService<IQueryRepository<Project, ProjectId>>();
        var entityCreated = await queryRepository.GetByIdAsync(new ProjectId(responseData.Id), CancellationToken.None);

        entityCreated.ShouldNotBeNull();
        entityCreated.ShouldSatisfyAllConditions(() => entityCreated.ProfileId.ShouldBe(profileEntity.Id));
    }

    private ProjectBase GetProjectData(Guid profileEntityId)
    {
        return _fixture
            .Build<ProjectBase>()
            .With(x => x.ProfileId, profileEntityId)
            .Create();
    }

    [Fact(DisplayName = "Return the record and return 200 when GET request with valid ID is provided")]
    public async Task ReturnRecord_WhenValidIdIsProvidedAsync()
    {
        // --- Given ---
        var entity = await SetupEntityAsync();

        // --- When ---
        var response = await _httpClient.GetAsync($"{RequestUriController}/{entity.Id}");

        // --- Then ---
        response.IsSuccessStatusCode.ShouldBeTrue();
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var responseData = await TestUtilities.DeserializeResponseAsync<ProjectResponse>(response);
        responseData.ShouldNotBeNull();
        responseData.Should().BeEquivalentTo(entity, options => options.ExcludingMissingMembers());
    }

    [Fact(DisplayName = "Return 404 when GET request with invalid ID is provided")]
    public async Task ReturnNotFound_WhenInvalidIdIsProvidedAsync()
    {
        // --- Given ---
        _ = await SetupEntityAsync();

        // --- When ---
        var response = await _httpClient.GetAsync($"{RequestUriController}/{InvalidId}");

        // --- Then ---
        response.IsSuccessStatusCode.ShouldBeFalse();
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    [Fact(DisplayName = "Delete and return 204 when DELETE request is received")]
    public async Task DeleteRecord_WhenDeleteRequestIsReceivedAsync()
    {
        // --- Given ---
        var entity = await SetupEntityAsync();

        // --- When ---
        var response = await _httpClient.DeleteAsync($"{RequestUriController}/{entity.Id}");

        // --- Then ---
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);

        await using var scope = _testFixture.GetProvider().CreateAsyncScope();
        var queryProjectRepository =
            scope.ServiceProvider.GetRequiredService<IQueryRepository<Project, ProjectId>>();

        var projectEntity = await queryProjectRepository.GetByIdAsync(entity.Id, CancellationToken.None);
        projectEntity.ShouldBeNull();
    }

    [Fact(DisplayName = "Return 404 when a DELETE request with invalid ID is provided")]
    public async Task ReturnNotFound_WhenDeleteRequestWithInvalidIdIsProvidedAsync()
    {
        // --- Given ---
        _ = await SetupEntityAsync();

        // --- When ---
        var response = await _httpClient.DeleteAsync($"{RequestUriController}/{InvalidId}");

        // --- Then ---
        response.IsSuccessStatusCode.ShouldBeFalse();
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    [Fact(DisplayName = "Return 204 when a valid PUT request is provided")]
    public async Task ReturnNotContent_WhenReceivedValidPutRequestAsync()
    {
        // --- Given ---
        var entity = await SetupEntityAsync();
        var updatedEntity = _fixture.Build<ProjectCommand>()
            .With(x => x.ProjectData, GetProjectData(entity.Id.Value))
            .Create();

        // --- When ---
        var response = await _httpClient.PutAsJsonAsync($"{RequestUriController}/{entity.Id}", updatedEntity);

        // --- Then ---
        response.IsSuccessStatusCode.ShouldBeTrue();
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);

        await using var scope = _testFixture.GetProvider().CreateAsyncScope();
        var queryRepository =
            scope.ServiceProvider.GetRequiredService<IQueryRepository<Project, ProjectId>>();
        var dbEntity = await queryRepository.GetByIdAsync(entity.Id, CancellationToken.None);
        dbEntity.ShouldNotBeNull();
        dbEntity.Should().BeEquivalentTo(updatedEntity.ProjectData);
    }

    [Fact(DisplayName = "Return 404 when a PUT request with invalid ID is provided")]
    public async Task ReturnNotFound_WhenPutRequestWithInvalidIdIsProvidedAsync()
    {
        // --- Given ---
        await _testFixture.ResetDatabaseAsync();
        var updatedEntity = _fixture.Build<ProjectCommand>().Create();

        // --- When ---
        var response = await _httpClient.PutAsJsonAsync($"{RequestUriController}/{InvalidId}", updatedEntity);

        // --- Then ---
        response.IsSuccessStatusCode.ShouldBeFalse();
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    private async Task<Profile> SetupProfileEntityAsync()
    {
        await _testFixture.ResetDatabaseAsync();
        await using var scope = _testFixture.GetProvider().CreateAsyncScope();
        var commandRepository = scope.ServiceProvider.GetRequiredService<ICommandRepository<Profile>>();

        var entity = new ProfileEntityBuilder(_fixture).WithProjects().Build();
        await commandRepository.CreateAsync(entity);
        await commandRepository.SaveChangesAsync();

        return entity;
    }

    private async Task<Project> SetupEntityAsync()
    {
        await _testFixture.ResetDatabaseAsync();
        return await CreateAndPersistEntityAsync();
    }

    private async Task<Project> CreateAndPersistEntityAsync()
    {
        await using var scope = _testFixture.GetProvider().CreateAsyncScope();
        var commandRepository = scope.ServiceProvider.GetRequiredService<ICommandRepository<Profile>>();

        var entity = new ProfileEntityBuilder(_fixture)
            .WithProjects()
            .Build();
        await commandRepository.CreateAsync(entity);
        await commandRepository.SaveChangesAsync();

        return entity.Projects.First();
    }
}