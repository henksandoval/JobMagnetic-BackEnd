using System.Net;
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
    private readonly ITestOutputHelper _testOutputHelper;

    public AboutControllerTests(JobMagnetTestSetupFixture testFixture, ITestOutputHelper testOutputHelper)
    {
        _testFixture = testFixture;
        _testOutputHelper = testOutputHelper;
        _httpClient = _testFixture.GetClient();
        _testFixture.SetTestOutputHelper(testOutputHelper);
    }

    [Fact(DisplayName = "Should return the record and return 200 when a valid ID is provided")]
    public async Task ShouldReturnRecord_WhenValidIdIsProvidedAsync()
    {
        await _testFixture.ResetDatabaseAsync();
        _testOutputHelper.WriteLine("Executing test: {0} in time: {1}",
            nameof(ShouldReturnRecord_WhenValidIdIsProvidedAsync), DateTime.Now);
        var entity = await CreateAndPersistEntityAsync();

        var response = await _httpClient.GetAsync($"{RequestUriController}/{entity.Id}");

        response.IsSuccessStatusCode.ShouldBeTrue();
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var responseData = await TestUtilities.DeserializeResponseAsync<AboutModel>(response);
        responseData.ShouldNotBeNull();
        responseData.Should().BeEquivalentTo(entity, options => options.ExcludingMissingMembers());
    }

    [Fact(DisplayName = "Should return 404 when a invalid ID is provided")]
    public async Task ShouldReturnNotFound_WhenInvalidIdIsProvidedAsync()
    {
        await _testFixture.ResetDatabaseAsync();
        _testOutputHelper.WriteLine("Executing test: {0} in time: {1}",
            nameof(ShouldReturnNotFound_WhenInvalidIdIsProvidedAsync), DateTime.Now);
        _ = await CreateAndPersistEntityAsync();

        var response = await _httpClient.GetAsync($"{RequestUriController}/100");

        response.IsSuccessStatusCode.ShouldBeFalse();
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    [Fact(DisplayName = "Should create a new record and return 201 when the request is valid")]
    public async Task ShouldReturnCreatedAndPersistData_WhenRequestIsValidAsync()
    {
        await _testFixture.ResetDatabaseAsync();
        _testOutputHelper.WriteLine("Executing test: {0} in time: {1}",
            nameof(ShouldReturnCreatedAndPersistData_WhenRequestIsValidAsync), DateTime.Now);
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

    private async Task<AboutEntity> CreateAndPersistEntityAsync()
    {
        await using var scope = _testFixture.GetProvider().CreateAsyncScope();
        var commandRepository = scope.ServiceProvider.GetRequiredService<ICommandRepository<AboutEntity>>();

        var entity = _fixture.Build<AboutEntity>().With(x => x.Id, 0).Create();
        await commandRepository.CreateAsync(entity);

        return entity;
    }
}