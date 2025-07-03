using System.Net;
using System.Net.Http.Json;
using AutoFixture;
using AwesomeAssertions;
using JobMagnet.Application.Contracts.Commands.Project;
using JobMagnet.Application.Contracts.Responses.Base;
using JobMagnet.Application.Contracts.Responses.Portfolio;
using JobMagnet.Application.Mappers;
using JobMagnet.Domain.Aggregates.Profiles;
using JobMagnet.Domain.Aggregates.Profiles.Entities;
using JobMagnet.Domain.Ports.Repositories.Base;
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
        var projectData = GetProjectData(profile.Id.Value);
        var createRequest = _fixture.Build<ProjectCommand>()
            .With(x => x.ProjectData, projectData)
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

        var entityCreated = await FindProjectByIdAsync(responseData.Id);

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
    public async Task GetProjectsNotFound_WhenProfileDoesNotExist()
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
        _projectCount = 0;
        var profileWithoutProjects = await SetupProfileAsync();

        // --- When ---
        var response = await _httpClient.GetAsync($"{RequestUriController}/{profileWithoutProjects.Id.Value}/projects");

        // --- Then ---
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var responseData = await TestUtilities.DeserializeResponseAsync<List<ProjectResponse>>(response);
        responseData.Should().NotBeNull().And.BeEmpty();
    }

    [Fact(DisplayName = "Should return 204 No Content when updating an existing project")]
    public async Task UpdateProject_WhenProfileAndProjectExistAndPayloadIsValid()
    {
        // --- Given ---
        _projectCount = 10;
        var profile = await SetupProfileAsync();
        var projectData = GetProjectData(profile.Id.Value);
        var projectToUpdate = profile.Projects.First();
        var command = _fixture.Build<ProjectCommand>()
            .With(x => x.ProjectData, projectData)
            .Create();
        var requestUri = $"{RequestUriController}/{profile.Id.Value}/projects/{projectToUpdate.Id.Value}";

        // --- When ---
        var response = await _httpClient.PutAsJsonAsync(requestUri, command);

        // --- Then ---
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        var projectUpdated = await FindProjectByIdAsync(projectToUpdate.Id.Value);

        projectUpdated.Should().NotBeNull();
        projectUpdated.ProfileId.Should().Be(profile.Id);
        var commandBase = command.ProjectData;
        projectUpdated.Should().BeEquivalentTo(commandBase, options => options
            .Excluding(expect => expect!.ProfileId)
            .Excluding(expect => expect!.Position)
        );
    }

    [Fact(DisplayName = "Should return 404 Not Found when the profile ID does not exist for an update")]
    public async Task UpdateProject_WhenProfileDoesNotExist()
    {
        // --- Given ---
        var nonExistentProfileId = Guid.NewGuid();
        var projectData = GetProjectData(nonExistentProfileId);
        var requestUri = $"{RequestUriController}/{nonExistentProfileId}/projects/{nonExistentProfileId}";

        // --- When ---
        var response = await _httpClient.PutAsJsonAsync(requestUri, projectData);

        // --- Then ---
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact(DisplayName = "Should return 404 Not Found when the project does not belong to the profile")]
    public async Task UpdateProject_WhenProjectDoesNotExistInProfile()
    {
        // --- Given ---
        var profile = await SetupProfileAsync();
        var projectData = GetProjectData(profile.Id.Value);
        var nonExistentProjectId = Guid.NewGuid();
        var command = _fixture.Build<ProjectCommand>()
            .With(x => x.ProjectData, projectData)
            .Create();
        var requestUri = $"{RequestUriController}/{profile.Id.Value}/projects/{nonExistentProjectId}";

        // --- When ---
        var response = await _httpClient.PutAsJsonAsync(requestUri, command);

        // --- Then ---
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact(DisplayName = "Should return 204 No Content when deleting an existing project")]
    public async Task DeleteProject_WhenProfileAndProjectExist()
    {
        // --- Given ---
        _projectCount = 5;
        var profile = await SetupProfileAsync();
        var projectToDelete = profile.Projects.First();
        var requestUri = $"{RequestUriController}/{profile.Id.Value}/projects/{projectToDelete.Id.Value}";

        // --- When ---
        var response = await _httpClient.DeleteAsync(requestUri);

        // --- Then ---
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        await using var scope = _testFixture.GetProvider().CreateAsyncScope();
        var queryRepository = scope.ServiceProvider.GetRequiredService<IQueryRepository<Project, ProjectId>>();
        var projects = await queryRepository.FindAsync(p => p.ProfileId == projectToDelete.ProfileId, CancellationToken.None);

        projects.Should().HaveCount(_projectCount - 1);
        projects.Should().NotContain(p => p.Id == projectToDelete.Id);
    }

    [Fact(DisplayName = "Should return 404 Not Found when the profile ID does not exist for a delete")]
    public async Task DeleteProject_WhenProfileDoesNotExist()
    {
        // --- Given ---
        var nonExistentProfileId = Guid.NewGuid();
        var requestUri = $"{RequestUriController}/{nonExistentProfileId}/projects/{Guid.NewGuid()}";

        // --- When ---
        var response = await _httpClient.DeleteAsync(requestUri);

        // --- Then ---
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact(DisplayName = "Should return 404 Not Found when deleting a project that does not belong to the profile")]
    public async Task DeleteProject_WhenProjectDoesNotExistInProfile()
    {
        // --- Given ---
        var profile = await SetupProfileAsync();
        var nonExistentProjectId = Guid.NewGuid();
        var requestUri = $"{RequestUriController}/{profile.Id.Value}/projects/{nonExistentProjectId}";

        // --- When ---
        var response = await _httpClient.DeleteAsync(requestUri);

        // --- Then ---
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact(DisplayName = "Should return 204 No Content when arranging projects with a valid order")]
    public async Task ArrangeProjects_WhenProfileExistsAndPayloadIsValid_ShouldUpdatePositionsInDb()
    {
        // --- Given ---
        _projectCount = 3;
        var profile = await SetupProfileAsync();
        var projects = profile.Projects.ToList();
        var newOrderExpected = projects.OrderByDescending(x => x.Position).Select(x => x.Id).ToList();
        var newOrderCommand = newOrderExpected.Select(x => x.Value);

        var requestUri = $"{RequestUriController}/{profile.Id.Value}/projects/arrange";

        // --- When ---
        var response = await _httpClient.PutAsJsonAsync(requestUri, newOrderCommand);

        // --- Then ---
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        var projectsUpdated = await FindProjectsByProfileIdAsync(profile.Id);
        var projectsOrderedByPosition = projectsUpdated.OrderBy(p => p.Position).Select(x => x.Id);
        projectsOrderedByPosition.Should().BeEquivalentTo(newOrderExpected);
    }

    private ProjectBase GetProjectData(Guid profileEntityId)
    {
        return _fixture
            .Build<ProjectBase>()
            .With(x => x.ProfileId, profileEntityId)
            .Create();
    }

    private async Task<Project?> FindProjectByIdAsync(Guid id)
    {
        await using var scope = _testFixture.GetProvider().CreateAsyncScope();
        var queryRepository = scope.ServiceProvider.GetRequiredService<IQueryRepository<Project, ProjectId>>();
        var entityCreated = await queryRepository.GetByIdAsync(new ProjectId(id), CancellationToken.None);
        return entityCreated;
    }

    private async Task<List<Project>> FindProjectsByProfileIdAsync(ProfileId profileId)
    {
        await using var scope = _testFixture.GetProvider().CreateAsyncScope();
        var queryRepository = scope.ServiceProvider.GetRequiredService<IQueryRepository<Project, ProjectId>>();

        var projects = await queryRepository.FindAsync(p => p.ProfileId == profileId, CancellationToken.None);
        return projects.ToList();
    }
}