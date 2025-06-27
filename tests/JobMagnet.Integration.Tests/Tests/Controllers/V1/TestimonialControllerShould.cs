using System.Net;
using System.Net.Http.Json;
using AutoFixture;
using AwesomeAssertions;
using JobMagnet.Application.Contracts.Commands.Testimonial;
using JobMagnet.Application.Contracts.Responses.Base;
using JobMagnet.Application.Contracts.Responses.Testimonial;
using JobMagnet.Domain.Aggregates.Profiles;
using JobMagnet.Domain.Aggregates.Profiles.Entities;
using JobMagnet.Domain.Ports.Repositories.Base;
using JobMagnet.Integration.Tests.Fixtures;
using JobMagnet.Shared.Tests.Fixtures;
using JobMagnet.Shared.Tests.Fixtures.Builders;
using JobMagnet.Shared.Tests.Utils;
using Microsoft.Extensions.DependencyInjection;

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
        // --- Given ---
        var entity = await SetupEntityAsync();

        // --- When ---
        var response = await _httpClient.GetAsync($"{RequestUriController}/{entity.Id}");

        // --- Then ---
        response.IsSuccessStatusCode.Should().BeTrue();
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var responseData = await TestUtilities.DeserializeResponseAsync<TestimonialResponse>(response);
        responseData.Should().NotBeNull();
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
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact(DisplayName = "Create a new record and return 201 when the POST request is valid")]
    public async Task ReturnCreatedAndPersistData_WhenRequestIsValidAsync()
    {
        // --- Given ---
        await _testFixture.ResetDatabaseAsync();
        var profileEntity = await SetupProfileEntityAsync();
        var createRequest = _fixture
            .Build<TestimonialCommand>()
            .With(x => x.TestimonialData, GetTestimonialData(profileEntity.Id.Value))
            .Create();
        var httpContent = TestUtilities.SerializeRequestContent(createRequest);

        // --- When ---
        var response = await _httpClient.PostAsync(RequestUriController, httpContent);

        // --- Then ---
        response.IsSuccessStatusCode.Should().BeTrue();
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var responseData = await TestUtilities.DeserializeResponseAsync<TestimonialResponse>(response);
        responseData.Should().NotBeNull();

        var locationHeader = response.Headers.Location!.ToString();
        locationHeader.Should().NotBeNull();
        locationHeader.Should().Contain($"{RequestUriController}/{responseData.Id}");

        await using var scope = _testFixture.GetProvider().CreateAsyncScope();
        var queryRepository = scope.ServiceProvider.GetRequiredService<IQueryRepository<Testimonial, TestimonialId>>();
        var entityCreated = await queryRepository.GetByIdAsync(new TestimonialId(responseData.Id), CancellationToken.None);

        entityCreated.Should().NotBeNull();
        entityCreated.Should()
            .BeEquivalentTo(createRequest.TestimonialData, options => options.ExcludingMissingMembers());
    }

    [Fact(DisplayName = "Should delete and return 204 when DELETE request is received")]
    public async Task DeleteRecord_WhenDeleteRequestIsReceivedAsync()
    {
        // --- Given ---
        var entity = await SetupEntityAsync();

        // --- When ---
        var response = await _httpClient.DeleteAsync($"{RequestUriController}/{entity.Id}");

        // --- Then ---
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        await using var scope = _testFixture.GetProvider().CreateAsyncScope();
        var queryRepository = scope.ServiceProvider.GetRequiredService<IQueryRepository<Testimonial, TestimonialId>>();
        var dbEntity = await queryRepository.GetByIdAsync(entity.Id, CancellationToken.None);
        dbEntity.Should().BeNull();
    }

    [Fact(DisplayName = "Return 404 when a DELETE request with invalid ID is provided")]
    public async Task ReturnNotFound_WhenDeleteRequestWithInvalidIdIsProvidedAsync()
    {
        // --- Given ---
        _ = await SetupEntityAsync();

        // --- When ---
        var response = await _httpClient.DeleteAsync($"{RequestUriController}/{InvalidId}");

        // --- Then ---
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact(DisplayName = "Return 204 when a valid PUT request is provided")]
    public async Task ReturnNotContent_WhenReceivedValidPutRequestAsync()
    {
        // --- Given ---
        var entity = await SetupEntityAsync();
        var updatedCommand = _fixture.Build<TestimonialCommand>()
            .With(x => x.TestimonialData, GetTestimonialData(entity.Id.Value))
            .Create();

        // --- When ---
        var response = await _httpClient.PutAsJsonAsync($"{RequestUriController}/{entity.Id}", updatedCommand);

        // --- Then ---
        response.IsSuccessStatusCode.Should().BeTrue();
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        await using var scope = _testFixture.GetProvider().CreateAsyncScope();
        var queryRepository = scope.ServiceProvider.GetRequiredService<IQueryRepository<Testimonial, TestimonialId>>();
        var dbEntity = await queryRepository.GetByIdAsync(entity.Id, CancellationToken.None);
        dbEntity.Should().NotBeNull();
        dbEntity.Should().BeEquivalentTo(updatedCommand.TestimonialData);
    }

    [Fact(DisplayName = "Return 404 when a PUT request with invalid ID is provided")]
    public async Task ReturnNotFound_WhenPutRequestWithInvalidIdIsProvidedAsync()
    {
        // --- Given ---
        await _testFixture.ResetDatabaseAsync();
        var updatedEntity = _fixture.Build<TestimonialCommand>().Create();

        // --- When ---
        var response = await _httpClient.PutAsJsonAsync($"{RequestUriController}/{InvalidId}", updatedEntity);

        // --- Then ---
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    private async Task<Testimonial> CreateAndPersistEntityAsync()
    {
        await using var scope = _testFixture.GetProvider().CreateAsyncScope();
        var commandRepository = scope.ServiceProvider.GetRequiredService<ICommandRepository<Profile>>();

        var entity = new ProfileEntityBuilder(_fixture)
            .WithTestimonials()
            .Build();
        await commandRepository.CreateAsync(entity);
        await commandRepository.SaveChangesAsync();

        return entity.Testimonials.First();
    }

    private async Task<Testimonial> SetupEntityAsync()
    {
        await _testFixture.ResetDatabaseAsync();
        return await CreateAndPersistEntityAsync();
    }

    private async Task<Profile> SetupProfileEntityAsync()
    {
        await _testFixture.ResetDatabaseAsync();
        await using var scope = _testFixture.GetProvider().CreateAsyncScope();
        var commandRepository = scope.ServiceProvider.GetRequiredService<ICommandRepository<Profile>>();

        var entity = new ProfileEntityBuilder(_fixture).Build();
        await commandRepository.CreateAsync(entity);
        await commandRepository.SaveChangesAsync();

        return entity;
    }

    private TestimonialBase GetTestimonialData(Guid profileEntityId)
    {
        return _fixture
            .Build<TestimonialBase>()
            .With(x => x.ProfileId, profileEntityId)
            .Create();
    }
}