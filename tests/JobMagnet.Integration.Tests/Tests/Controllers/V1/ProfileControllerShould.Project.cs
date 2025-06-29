using System.Net;
using AutoFixture;
using AwesomeAssertions;
using JobMagnet.Application.Contracts.Commands.Portfolio;
using JobMagnet.Application.Contracts.Responses.Base;
using JobMagnet.Application.Contracts.Responses.Portfolio;
using JobMagnet.Application.Mappers;
using JobMagnet.Domain.Aggregates.Profiles;
using JobMagnet.Domain.Aggregates.Profiles.Entities;
using JobMagnet.Domain.Ports.Repositories.Base;
using JobMagnet.Shared.Tests.Fixtures.Builders;
using JobMagnet.Shared.Tests.Utils;
using Microsoft.Extensions.DependencyInjection;

namespace JobMagnet.Integration.Tests.Tests.Controllers.V1;

public partial class ProfileControllerShould
{
    [Fact(DisplayName = "Should return 201 Created with the new project when payload is valid for an existing profile")]
    public async Task AddProject_WhenProfileExistsAndPayloadIsValid()
    {
        // --- Given ---
        var profile = await SetupProfileAsync();
        var createRequest = _fixture.Build<ProjectCommand>()
            .With(x => x.ProjectData, GetProjectData(profile.Id.Value))
            .Create();
        var httpContent = TestUtilities.SerializeRequestContent(createRequest);

        // --- When ---
        var response = await _httpClient.PostAsync($"{RequestUriController}/{profile.Id.Value}/project", httpContent);

        // --- Then ---
        response.IsSuccessStatusCode.Should().BeTrue();
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var responseData = await TestUtilities.DeserializeResponseAsync<ProjectResponse>(response);
        responseData.Should().NotBeNull();

        var locationHeader = response.Headers.Location!.ToString();
        locationHeader.Should().NotBeNull();
        var expectedHeader = $"{RequestUriController}/{profile.Id.Value}/projects";
        locationHeader.Should().Match(currentHeader =>
            currentHeader.Contains(expectedHeader, StringComparison.OrdinalIgnoreCase)
        );

        await using var scope = _testFixture.GetProvider().CreateAsyncScope();
        var queryRepository = scope.ServiceProvider.GetRequiredService<IQueryRepository<Project, ProjectId>>();
        var entityCreated = await queryRepository.GetByIdAsync(new ProjectId(responseData.Id), CancellationToken.None);

        entityCreated.Should().NotBeNull();
        entityCreated.ProfileId.Should().Be(profile.Id);
    }

    [Fact(DisplayName = "Should return 200 OK with a list of projects when the profile exists and has projects")]
    public async Task GetProjects_WhenProfileExistsAndHasProjects()
    {
        // --- Given ---
        var profileWithProjects = await SetupProfileAsync();
        var expectedProjects = profileWithProjects.Projects.Select(p => p.ToModel());

        // --- When ---
        var response = await _httpClient.GetAsync($"{RequestUriController}/{profileWithProjects.Id.Value}/projects");

        // --- Then ---
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var responseData = await TestUtilities.DeserializeResponseAsync<List<ProjectResponse>>(response);
        responseData.Should().NotBeNull();
        responseData.Should().BeEquivalentTo(expectedProjects);
    }

    [Fact(DisplayName = "Should return 404 Not Found when the profile ID does not exist")]
    public async Task GetNotFound_WhenProfileDoesNotExist()
    {
        // --- Given ---
        var nonExistentProfileId = Guid.NewGuid();
        await _testFixture.ResetDatabaseAsync();

        // --- When ---
        var response = await _httpClient.GetAsync($"{RequestUriController}/{nonExistentProfileId}/projects");

        // --- Then ---
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact(DisplayName = "Should return 200 OK with an empty list when the profile exists but has no projects")]
    public async Task GetOkAndEmptyList_WhenProfileExistsButHasNoProjects()
    {
        // --- Given ---
        var profileWithoutProjects = await CreateAndPersistProfileWithoutProjectsAsync();

        // --- When ---
        var response = await _httpClient.GetAsync($"{RequestUriController}/{profileWithoutProjects.Id.Value}/projects");

        // --- Then ---
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var responseData = await TestUtilities.DeserializeResponseAsync<List<ProjectResponse>>(response);
        responseData.Should().NotBeNull().And.BeEmpty();
    }

    private async Task<Profile> CreateAndPersistProfileWithoutProjectsAsync()
    {
        await _testFixture.ResetDatabaseAsync();

        await using var scope = _testFixture.GetProvider().CreateAsyncScope();
        var commandRepository = scope.ServiceProvider.GetRequiredService<ICommandRepository<Profile>>();
        var unitWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

        var entity = new ProfileEntityBuilder(_fixture)
            .WithProjects(0)
            .Build();

        await commandRepository.CreateAsync(entity);
        await unitWork.SaveChangesAsync();

        return entity;
    }

    private ProjectBase GetProjectData(Guid profileEntityId)
    {
        return _fixture
            .Build<ProjectBase>()
            .With(x => x.ProfileId, profileEntityId)
            .Create();
    }
}