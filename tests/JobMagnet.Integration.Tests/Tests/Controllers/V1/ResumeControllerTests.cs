using System.Net;
using System.Net.Http.Json;
using AutoFixture;
using FluentAssertions;
using JobMagnet.Infrastructure.Entities;
using JobMagnet.Infrastructure.Repositories.Base.Interfaces;
using JobMagnet.Integration.Tests.Extensions;
using JobMagnet.Integration.Tests.Fixtures;
using JobMagnet.Integration.Tests.Utils;
using JobMagnet.Models.Resume;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit.Abstractions;

namespace JobMagnet.Integration.Tests.Tests.Controllers.V1;

public class ResumeControllerTests : IClassFixture<JobMagnetTestSetupFixture>
{
    private const string RequestUriController = "api/v1/resume";
    private const string InvalidId = "100";
    private readonly IFixture _fixture = FixtureBuilder.Build();
    private readonly HttpClient _httpClient;
    private readonly JobMagnetTestSetupFixture _testFixture;

    public ResumeControllerTests(JobMagnetTestSetupFixture testFixture, ITestOutputHelper testOutputHelper)
    {
        _testFixture = testFixture;
        _httpClient = _testFixture.GetClient();
        _testFixture.SetTestOutputHelper(testOutputHelper);
    }

    [Fact(DisplayName = "Should return the record and return 200 when GET request with valid ID is provided")]
    public async Task ShouldReturnRecord_WhenValidIdIsProvidedAsync()
    {
        // Given
        var entity = await SetupEntityAsync();

        // When
        var response = await _httpClient.GetAsync($"{RequestUriController}/{entity.Id}");

        // Then
        response.IsSuccessStatusCode.ShouldBeTrue();
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var responseData = await TestUtilities.DeserializeResponseAsync<ResumeModel>(response);
        responseData.ShouldNotBeNull();
        responseData.Should().BeEquivalentTo(entity, options => options.ExcludingMissingMembers());
    }

    [Fact(DisplayName = "Should return 404 when GET request with invalid ID is provided")]
    public async Task ShouldReturnNotFound_WhenInvalidIdIsProvidedAsync()
    {
        // Given
        _ = await SetupEntityAsync();

        // When
        var response = await _httpClient.GetAsync($"{RequestUriController}/{InvalidId}");

        // Then
        response.IsSuccessStatusCode.ShouldBeFalse();
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    [Fact(DisplayName = "Should create a new record and return 201 when the POST request is valid")]
    public async Task ShouldReturnCreatedAndPersistData_WhenRequestIsValidAsync()
    {
        // Given
        await _testFixture.ResetDatabaseAsync();
        var entity = await SetupProfileEntityAsync();
        var createRequest = _fixture.Build<ResumeCreateRequest>().Create();
        createRequest.ProfileId = entity.Id;
        var httpContent = TestUtilities.SerializeRequestContent(createRequest);

        // When
        var response = await _httpClient.PostAsync(RequestUriController, httpContent);

        // Then
        response.IsSuccessStatusCode.ShouldBeTrue();
        response.StatusCode.ShouldBe(HttpStatusCode.Created);

        var responseData = await TestUtilities.DeserializeResponseAsync<ResumeModel>(response);
        responseData.ShouldNotBeNull();

        var locationHeader = response.Headers.Location!.ToString();
        locationHeader.ShouldNotBeNull();
        locationHeader.ShouldContain($"{RequestUriController}/{responseData.Id}");

        await using var scope = _testFixture.GetProvider().CreateAsyncScope();
        var queryRepository = scope.ServiceProvider.GetRequiredService<IQueryRepository<ResumeEntity, long>>();
        var entityCreated = await queryRepository.GetByIdAsync(responseData.Id);

        entityCreated.ShouldNotBeNull();
        entityCreated.Should().BeEquivalentTo(createRequest, options => options.ExcludingMissingMembers());
    }

    [Fact(DisplayName = "Should delete and return 204 when DELETE request is received")]
    public async Task ShouldDeleteRecord_WhenDeleteRequestIsReceivedAsync()
    {
        // Given
        var entity = await SetupEntityAsync();

        // When
        var response = await _httpClient.DeleteAsync($"{RequestUriController}/{entity.Id}");

        // Then
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);

        await using var scope = _testFixture.GetProvider().CreateAsyncScope();
        var queryRepository = scope.ServiceProvider.GetRequiredService<IQueryRepository<ResumeEntity, long>>();
        var dbEntity = await queryRepository.GetByIdAsync(entity.Id);
        dbEntity.ShouldBeNull();
    }

    [Fact(DisplayName = "Should return 404 when a DELETE request with invalid ID is provided")]
    public async Task ShouldReturnNotFound_WhenDeleteRequestWithInvalidIdIsProvidedAsync()
    {
        // Given
        _ = await SetupEntityAsync();

        // When
        var response = await _httpClient.DeleteAsync($"{RequestUriController}/{InvalidId}");

        // Then
        response.IsSuccessStatusCode.ShouldBeFalse();
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    [Fact(DisplayName = "Should return 204 when a valid PUT request is provided")]
    public async Task ShouldReturnNotContent_WhenReceivedValidPutRequestAsync()
    {
        // Given
        var entity = await SetupEntityAsync();
        var updateRequest = _fixture.Build<ResumeUpdateRequest>()
            .With(x => x.Id, entity.Id)
            .With(x => x.ProfileId, entity.ProfileId)
            .Create();

        // When
        var response = await _httpClient.PutAsJsonAsync($"{RequestUriController}/{entity.Id}", updateRequest);

        // Then
        response.IsSuccessStatusCode.ShouldBeTrue();
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);

        await using var scope = _testFixture.GetProvider().CreateAsyncScope();
        var queryRepository = scope.ServiceProvider.GetRequiredService<IQueryRepository<ResumeEntity, long>>();
        var dbEntity = await queryRepository.GetByIdAsync(entity.Id);
        dbEntity.ShouldNotBeNull();
        dbEntity.Should().BeEquivalentTo(updateRequest, options => options.ExcludingMissingMembers());
    }

    [Fact(DisplayName = "Should return 400 when a PUT request with invalid ID is provided")]
    public async Task ShouldReturnBadRequest_WhenPutRequestWithInvalidIdIsProvidedAsync()
    {
        // Given
        await _testFixture.ResetDatabaseAsync();
        var updatedEntity = _fixture.Build<ResumeUpdateRequest>().Create();
        var differentId = updatedEntity.Id + InvalidId;

        // When
        var response = await _httpClient.PutAsJsonAsync($"{RequestUriController}/{differentId}", updatedEntity);

        // Then
        response.IsSuccessStatusCode.ShouldBeFalse();
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }

    [Fact(DisplayName = "Should return 404 when a PUT request with invalid ID is provided")]
    public async Task ShouldReturnNotFound_WhenPutRequestWithInvalidIdIsProvidedAsync()
    {
        // Given
        await _testFixture.ResetDatabaseAsync();
        var updatedEntity = _fixture.Build<ResumeUpdateRequest>().Create();

        // When
        var response = await _httpClient.PutAsJsonAsync($"{RequestUriController}/{updatedEntity.Id}", updatedEntity);

        // Then
        response.IsSuccessStatusCode.ShouldBeFalse();
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    [Fact(DisplayName = "Should return 204 when a valid PATCH request is provided")]
    public async Task ShouldReturnNotContent_WhenReceivedValidPatchRequestAsync()
    {
        // Given
        const string newJobTitle = "Software developer";
        var entity = await SetupEntityAsync();
        var patchDocument = new JsonPatchDocument<ResumeUpdateRequest>();
        patchDocument.Replace(a => a.JobTitle, newJobTitle);

        // When
        var response =
            await _httpClient.PatchAsNewtonsoftJsonAsync($"{RequestUriController}/{entity.Id}", patchDocument);

        // Then
        response.IsSuccessStatusCode.ShouldBeTrue();
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);

        await using var scope = _testFixture.GetProvider().CreateAsyncScope();
        var queryRepository = scope.ServiceProvider.GetRequiredService<IQueryRepository<ResumeEntity, long>>();
        var dbEntity = await queryRepository.GetByIdAsync(entity.Id);
        dbEntity.ShouldNotBeNull();
        dbEntity.JobTitle.ShouldBe(newJobTitle);
    }

    [Fact(DisplayName = "Should return 404 when a PATCH request with invalid ID is provided")]
    public async Task ShouldReturnNotFound_WhenPatchRequestWithInvalidIdIsProvidedAsync()
    {
        // Given
        await _testFixture.ResetDatabaseAsync();
        var updatedEntity = _fixture.Build<ResumeUpdateRequest>().Create();
        var patchDocument = new JsonPatchDocument<ResumeUpdateRequest>();

        // When
        var response =
            await _httpClient.PatchAsNewtonsoftJsonAsync($"{RequestUriController}/{updatedEntity.Id}", patchDocument);

        // Then
        response.IsSuccessStatusCode.ShouldBeFalse();
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    private async Task<ProfileEntity> SetupProfileEntityAsync()
    {
        await _testFixture.ResetDatabaseAsync();
        await using var scope = _testFixture.GetProvider().CreateAsyncScope();
        var commandRepository = scope.ServiceProvider.GetRequiredService<ICommandRepository<ProfileEntity>>();

        var entity = _fixture.BuildProfileEntity();
        await commandRepository.CreateAsync(entity);

        return entity;
    }

    private async Task<ResumeEntity> SetupEntityAsync()
    {
        await _testFixture.ResetDatabaseAsync();
        return await CreateAndPersistEntityAsync();
    }

    private async Task<ResumeEntity> CreateAndPersistEntityAsync()
    {
        await using var scope = _testFixture.GetProvider().CreateAsyncScope();
        var commandRepository = scope.ServiceProvider.GetRequiredService<ICommandRepository<ResumeEntity>>();

        var entity = _fixture.BuildResumeEntity();
        await commandRepository.CreateAsync(entity);

        return entity;
    }
}