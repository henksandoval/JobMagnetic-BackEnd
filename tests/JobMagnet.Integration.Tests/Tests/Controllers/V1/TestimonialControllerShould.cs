using System.Net;
using System.Net.Http.Json;
using AutoFixture;
using FluentAssertions;
using JobMagnet.Application.Contracts.Commands.Testimonial;
using JobMagnet.Application.Contracts.Responses.Base;
using JobMagnet.Application.Contracts.Responses.Testimonial;
using JobMagnet.Domain.Core.Entities;
using JobMagnet.Domain.Ports.Repositories.Base;
using JobMagnet.Integration.Tests.Extensions;
using JobMagnet.Integration.Tests.Fixtures;
using JobMagnet.Shared.Tests.Fixtures;
using JobMagnet.Shared.Tests.Fixtures.Builders;
using JobMagnet.Shared.Tests.Utils;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit.Abstractions;

namespace JobMagnet.Integration.Tests.Tests.Controllers.V1;

public class TestimonialControllerShould : IClassFixture<JobMagnetTestSetupFixture>
{
    private const string RequestUriController = "api/v1/testimonial";
    private const string InvalidId = "100";
    private readonly IFixture _fixture = FixtureBuilder.Build();
    private readonly HttpClient _httpClient;
    private readonly JobMagnetTestSetupFixture _testFixture;

    public TestimonialControllerShould(JobMagnetTestSetupFixture testFixture, ITestOutputHelper testOutputHelper)
    {
        _testFixture = testFixture;
        _httpClient = _testFixture.GetClient();
        _testFixture.SetTestOutputHelper(testOutputHelper);
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

        var responseData = await TestUtilities.DeserializeResponseAsync<TestimonialResponse>(response);
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

    [Fact(DisplayName = "Create a new record and return 201 when the POST request is valid")]
    public async Task ReturnCreatedAndPersistData_WhenRequestIsValidAsync()
    {
        // Given
        await _testFixture.ResetDatabaseAsync();
        var profileEntity = await SetupProfileEntityAsync();
        var createRequest = _fixture
            .Build<TestimonialCommand>()
            .With(x => x.TestimonialData, GetTestimonialData(profileEntity.Id))
            .Create();
        var httpContent = TestUtilities.SerializeRequestContent(createRequest);

        // When
        var response = await _httpClient.PostAsync(RequestUriController, httpContent);

        // Then
        response.IsSuccessStatusCode.ShouldBeTrue();
        response.StatusCode.ShouldBe(HttpStatusCode.Created);

        var responseData = await TestUtilities.DeserializeResponseAsync<TestimonialResponse>(response);
        responseData.ShouldNotBeNull();

        var locationHeader = response.Headers.Location!.ToString();
        locationHeader.ShouldNotBeNull();
        locationHeader.ShouldContain($"{RequestUriController}/{responseData.Id}");

        await using var scope = _testFixture.GetProvider().CreateAsyncScope();
        var queryRepository = scope.ServiceProvider.GetRequiredService<IQueryRepository<TestimonialEntity, long>>();
        var entityCreated = await queryRepository.GetByIdAsync(responseData.Id);

        entityCreated.ShouldNotBeNull();
        entityCreated.Should()
            .BeEquivalentTo(createRequest.TestimonialData, options => options.ExcludingMissingMembers());
    }

    [Fact(DisplayName = "Should delete and return 204 when DELETE request is received")]
    public async Task DeleteRecord_WhenDeleteRequestIsReceivedAsync()
    {
        // Given
        var entity = await SetupEntityAsync();

        // When
        var response = await _httpClient.DeleteAsync($"{RequestUriController}/{entity.Id}");

        // Then
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);

        await using var scope = _testFixture.GetProvider().CreateAsyncScope();
        var queryRepository = scope.ServiceProvider.GetRequiredService<IQueryRepository<TestimonialEntity, long>>();
        var dbEntity = await queryRepository.GetByIdAsync(entity.Id);
        dbEntity.ShouldBeNull();
    }

    [Fact(DisplayName = "Return 404 when a DELETE request with invalid ID is provided")]
    public async Task ReturnNotFound_WhenDeleteRequestWithInvalidIdIsProvidedAsync()
    {
        // Given
        _ = await SetupEntityAsync();

        // When
        var response = await _httpClient.DeleteAsync($"{RequestUriController}/{InvalidId}");

        // Then
        response.IsSuccessStatusCode.ShouldBeFalse();
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    [Fact(DisplayName = "Return 204 when a valid PUT request is provided")]
    public async Task ReturnNotContent_WhenReceivedValidPutRequestAsync()
    {
        // Given
        var entity = await SetupEntityAsync();
        var updatedCommand = _fixture.Build<TestimonialCommand>()
            .With(x => x.TestimonialData, GetTestimonialData(entity.Id))
            .Create();

        // When
        var response = await _httpClient.PutAsJsonAsync($"{RequestUriController}/{entity.Id}", updatedCommand);

        // Then
        response.IsSuccessStatusCode.ShouldBeTrue();
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);

        await using var scope = _testFixture.GetProvider().CreateAsyncScope();
        var queryRepository = scope.ServiceProvider.GetRequiredService<IQueryRepository<TestimonialEntity, long>>();
        var dbEntity = await queryRepository.GetByIdAsync(entity.Id);
        dbEntity.ShouldNotBeNull();
        dbEntity.Should().BeEquivalentTo(updatedCommand.TestimonialData);
    }

    [Fact(DisplayName = "Return 404 when a PUT request with invalid ID is provided")]
    public async Task ReturnNotFound_WhenPutRequestWithInvalidIdIsProvidedAsync()
    {
        // Given
        await _testFixture.ResetDatabaseAsync();
        var updatedEntity = _fixture.Build<TestimonialCommand>().Create();

        // When
        var response = await _httpClient.PutAsJsonAsync($"{RequestUriController}/{InvalidId}", updatedEntity);

        // Then
        response.IsSuccessStatusCode.ShouldBeFalse();
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    [Fact(DisplayName = "Return 204 when a valid PATCH request is provided")]
    public async Task ReturnNotContent_WhenReceivedValidPatchRequestAsync()
    {
        // Given
        const string newJobTitle = "Software developer";
        var entity = await SetupEntityAsync();
        var patchDocument = new JsonPatchDocument<TestimonialCommand>();
        patchDocument.Replace(a => a.TestimonialData.JobTitle, newJobTitle);

        // When
        var response =
            await _httpClient.PatchAsNewtonsoftJsonAsync($"{RequestUriController}/{entity.Id}", patchDocument);

        // Then
        response.IsSuccessStatusCode.ShouldBeTrue();
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);

        await using var scope = _testFixture.GetProvider().CreateAsyncScope();
        var queryRepository = scope.ServiceProvider.GetRequiredService<IQueryRepository<TestimonialEntity, long>>();
        var dbEntity = await queryRepository.GetByIdAsync(entity.Id);
        dbEntity.ShouldNotBeNull();
        dbEntity.JobTitle.ShouldBe(newJobTitle);
    }

    [Fact(DisplayName = "Return 404 when a PATCH request with invalid ID is provided")]
    public async Task ReturnNotFound_WhenPatchRequestWithInvalidIdIsProvidedAsync()
    {
        // Given
        await _testFixture.ResetDatabaseAsync();
        var patchDocument = new JsonPatchDocument<TestimonialCommand>();

        // When
        var response =
            await _httpClient.PatchAsNewtonsoftJsonAsync($"{RequestUriController}/{InvalidId}", patchDocument);

        // Then
        response.IsSuccessStatusCode.ShouldBeFalse();
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    private async Task<TestimonialEntity> CreateAndPersistEntityAsync()
    {
        await using var scope = _testFixture.GetProvider().CreateAsyncScope();
        var commandRepository = scope.ServiceProvider.GetRequiredService<ICommandRepository<TestimonialEntity>>();

        var entity = _fixture.Create<TestimonialEntity>();
        await commandRepository.CreateAsync(entity);
        await commandRepository.SaveChangesAsync();

        return entity;
    }

    private async Task<TestimonialEntity> SetupEntityAsync()
    {
        await _testFixture.ResetDatabaseAsync();
        return await CreateAndPersistEntityAsync();
    }

    private async Task<ProfileEntity> SetupProfileEntityAsync()
    {
        await _testFixture.ResetDatabaseAsync();
        await using var scope = _testFixture.GetProvider().CreateAsyncScope();
        var commandRepository = scope.ServiceProvider.GetRequiredService<ICommandRepository<ProfileEntity>>();

        var entity = new ProfileEntityBuilder(_fixture).Build();
        await commandRepository.CreateAsync(entity);
        await commandRepository.SaveChangesAsync();

        return entity;
    }

    private TestimonialBase GetTestimonialData(long profileEntityId)
    {
        return _fixture
            .Build<TestimonialBase>()
            .With(x => x.ProfileId, profileEntityId)
            .Create();
    }
}