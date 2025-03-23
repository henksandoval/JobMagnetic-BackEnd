using System.Net;
using System.Net.Http.Json;
using AutoFixture;
using FluentAssertions;
using JobMagnet.Entities;
using JobMagnet.Integration.Tests.Fixtures;
using JobMagnet.Integration.Tests.Utils;
using JobMagnet.Models;
using JobMagnet.Repositories.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit.Abstractions;

namespace JobMagnet.Integration.Tests.Tests.Controllers;

public class AboutControllerTests : IClassFixture<JobMagnetTestSetupFixture>
{
    private const string RequestUriController = "api/about";
    private readonly Fixture _fixture = new();
    private readonly HttpClient _httpClient;
    private readonly JobMagnetTestSetupFixture _testFixture;

    public AboutControllerTests(JobMagnetTestSetupFixture testFixture, ITestOutputHelper testOutputHelper)
    {
        _testFixture = testFixture;
        _httpClient = _testFixture.GetClient();
        _testFixture.SetTestOutputHelper(testOutputHelper);
    }

    [Fact(DisplayName = "Should return the record and return 200 when GET request with valid ID is provided")]
    public async Task ShouldReturnRecord_WhenValidIdIsProvidedAsync()
    {
        await _testFixture.ResetDatabaseAsync();
        var entity = await CreateAndPersistEntityAsync();

        var response = await _httpClient.GetAsync($"{RequestUriController}/{entity.Id}");

        response.IsSuccessStatusCode.ShouldBeTrue();
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var responseData = await TestUtilities.DeserializeResponseAsync<AboutModel>(response);
        responseData.ShouldNotBeNull();
        responseData.Should().BeEquivalentTo(entity, options => options.ExcludingMissingMembers());
    }

    [Fact(DisplayName = "Should return 404 when GET request with invalid ID is provided")]
    public async Task ShouldReturnNotFound_WhenInvalidIdIsProvidedAsync()
    {
        await _testFixture.ResetDatabaseAsync();
        _ = await CreateAndPersistEntityAsync();

        var response = await _httpClient.GetAsync($"{RequestUriController}/100");

        response.IsSuccessStatusCode.ShouldBeFalse();
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    [Fact(DisplayName = "Should create a new record and return 201 when the POST request is valid")]
    public async Task ShouldReturnCreatedAndPersistData_WhenRequestIsValidAsync()
    {
        await _testFixture.ResetDatabaseAsync();
        var createRequest = _fixture.Build<AboutCreateRequest>().Create();
        var httpContent = TestUtilities.SerializeRequestContent(createRequest);

        var response = await _httpClient.PostAsync(RequestUriController, httpContent);

        response.IsSuccessStatusCode.ShouldBeTrue();
        response.StatusCode.ShouldBe(HttpStatusCode.Created);

        var responseData = await TestUtilities.DeserializeResponseAsync<AboutModel>(response);
        responseData.ShouldNotBeNull();

        var locationHeader = response.Headers.Location!.ToString();
        locationHeader.ShouldNotBeNull();
        locationHeader.ShouldContain($"{RequestUriController}/{responseData.Id}");

        await using var scope = _testFixture.GetProvider().CreateAsyncScope();
        var queryRepository = scope.ServiceProvider.GetRequiredService<IQueryRepository<AboutEntity>>();
        var entityCreated = await queryRepository.GetByIdAsync(responseData.Id);

        entityCreated.ShouldNotBeNull();
        entityCreated.Should().BeEquivalentTo(createRequest, options => options.ExcludingMissingMembers());
    }

    [Fact(DisplayName = "Should delete and return 204 when DELETE request is received")]
    public async Task ShouldDeleteRecord_WhenDeleteRequestIsReceivedAsync()
    {
        await _testFixture.ResetDatabaseAsync();
        var entity = await CreateAndPersistEntityAsync();

        var response = await _httpClient.DeleteAsync($"{RequestUriController}/{entity.Id}");

        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);

        await using var scope = _testFixture.GetProvider().CreateAsyncScope();
        var queryRepository = scope.ServiceProvider.GetRequiredService<IQueryRepository<AboutEntity>>();
        var aboutEntity = await queryRepository.GetByIdAsync(entity.Id);
        aboutEntity.ShouldBeNull();
    }

    [Fact(DisplayName = "Should return 404 when a DELETE request with invalid ID is provided")]
    public async Task ShouldReturnNotFound_WhenDeleteRequestWithInvalidIdIsProvidedAsync()
    {
        await _testFixture.ResetDatabaseAsync();
        _ = await CreateAndPersistEntityAsync();

        var response = await _httpClient.DeleteAsync($"{RequestUriController}/100");

        response.IsSuccessStatusCode.ShouldBeFalse();
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    [Fact(DisplayName = "Should return 204 when a valid PUT request is provided")]
    public async Task ShouldReturnNotContent_WhenReceivedValidPutRequestAsync()
    {
        await _testFixture.ResetDatabaseAsync();
        var entity = await CreateAndPersistEntityAsync();
        var updatedEntity = _fixture.Build<AboutUpdateRequest>().With(x => x.Id, entity.Id).Create();

        var response = await _httpClient.PutAsJsonAsync($"{RequestUriController}/{entity.Id}", updatedEntity);

        response.IsSuccessStatusCode.ShouldBeTrue();
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);

        await using var scope = _testFixture.GetProvider().CreateAsyncScope();
        var queryRepository = scope.ServiceProvider.GetRequiredService<IQueryRepository<AboutEntity>>();
        var aboutEntity = await queryRepository.GetByIdAsync(entity.Id);
        aboutEntity.ShouldNotBeNull();
        aboutEntity.Should().BeEquivalentTo(updatedEntity);
    }

    [Fact(DisplayName = "Should return 400 when a PUT request with invalid ID is provided")]
    public async Task ShouldReturnBadRequest_WhenPutRequestWithInvalidIdIsProvidedAsync()
    {
        await _testFixture.ResetDatabaseAsync();
        var updatedEntity = _fixture.Build<AboutUpdateRequest>().Create();
        var differentId = updatedEntity.Id + 100;

        var response = await _httpClient.PutAsJsonAsync($"{RequestUriController}/{differentId}", updatedEntity);

        response.IsSuccessStatusCode.ShouldBeFalse();
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }

    [Fact(DisplayName = "Should return 404 when a PUT request with invalid ID is provided")]
    public async Task ShouldReturnNotFound_WhenPutRequestWithInvalidIdIsProvidedAsync()
    {
        await _testFixture.ResetDatabaseAsync();
        var updatedEntity = _fixture.Build<AboutUpdateRequest>().Create();

        var response = await _httpClient.PutAsJsonAsync($"{RequestUriController}/{updatedEntity.Id}", updatedEntity);

        response.IsSuccessStatusCode.ShouldBeFalse();
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    [Fact(DisplayName = "Should return 204 when a valid PATCH request is provided")]
    public async Task ShouldReturnNotContent_WhenReceivedValidPatchRequestAsync()
    {
        await _testFixture.ResetDatabaseAsync();
        var entity = await CreateAndPersistEntityAsync();
        var updatedEntity = _fixture.Build<AboutUpdateRequest>().With(x => x.Id, entity.Id).Create();

        var response = await _httpClient.PatchAsJsonAsync($"{RequestUriController}/{entity.Id}", updatedEntity);

        response.IsSuccessStatusCode.ShouldBeTrue();
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);

        await using var scope = _testFixture.GetProvider().CreateAsyncScope();
        var queryRepository = scope.ServiceProvider.GetRequiredService<IQueryRepository<AboutEntity>>();
        var aboutEntity = await queryRepository.GetByIdAsync(entity.Id);
        aboutEntity.ShouldNotBeNull();
        aboutEntity.Should().BeEquivalentTo(updatedEntity);
    }

    private async Task<AboutEntity> CreateAndPersistEntityAsync()
    {
        await using var scope = _testFixture.GetProvider().CreateAsyncScope();
        var commandRepository = scope.ServiceProvider.GetRequiredService<ICommandRepository<AboutEntity>>();

        var entity = _fixture.Build<AboutEntity>().With(x => x.Id, 0).Create();
        await commandRepository.CreateAsync(entity);

        return entity;
    }
}