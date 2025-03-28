using System.Net;
using AutoFixture;
using FluentAssertions;
using JobMagnet.Infrastructure.Entities;
using JobMagnet.Infrastructure.Repositories.Base.Interfaces;
using JobMagnet.Infrastructure.Repositories.Interfaces;
using JobMagnet.Integration.Tests.Fixtures;
using JobMagnet.Integration.Tests.Utils;
using JobMagnet.Models.Portfolio;
using JobMagnet.Models.Resume;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit.Abstractions;

namespace JobMagnet.Integration.Tests.Tests.Controllers;

public class PortfolioControllerTests : IClassFixture<JobMagnetTestSetupFixture>
{
    private const string RequestUriController = "api/Portfolio";
    private const string InvalidId = "100";
    private readonly IFixture _fixture = FixtureBuilder.Build();
    private readonly HttpClient _httpClient;
    private readonly JobMagnetTestSetupFixture _testFixture;

    public PortfolioControllerTests(JobMagnetTestSetupFixture testFixture, ITestOutputHelper testOutputHelper)
    {
        _testFixture = testFixture;
        _httpClient = _testFixture.GetClient();
        _testFixture.SetTestOutputHelper(testOutputHelper);
    }

    [Fact(DisplayName = "Should create a new record and return 201 when the POST request is valid")]
    public async Task ShouldReturnCreatedAndPersistData_WhenRequestIsValidAsync()
    {
        // Given
        await _testFixture.ResetDatabaseAsync();
        var createRequest = _fixture.Build<PortfolioCreateRequest>().Create();
        var httpContent = TestUtilities.SerializeRequestContent(createRequest);

        // When
        var response = await _httpClient.PostAsync(RequestUriController, httpContent);

        // Then
        response.IsSuccessStatusCode.ShouldBeTrue();
        response.StatusCode.ShouldBe(HttpStatusCode.Created);

        var responseData = await TestUtilities.DeserializeResponseAsync<PortfolioModel>(response);
        responseData.ShouldNotBeNull();

        var locationHeader = response.Headers.Location!.ToString();
        locationHeader.ShouldNotBeNull();
        locationHeader.ShouldContain($"{RequestUriController}/{responseData.Id}");

        await using var scope = _testFixture.GetProvider().CreateAsyncScope();
        var queryRepository = scope.ServiceProvider.GetRequiredService<IPortfolioQueryRepository>();
        _ = queryRepository.IncludeGalleryItems();
        var entityCreated = await queryRepository.GetByIdWithIncludesAsync(responseData.Id);

        entityCreated.ShouldNotBeNull();
        entityCreated.Should().BeEquivalentTo(createRequest, options => options.ExcludingMissingMembers());
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
        var queryPortfolioRepository = scope.ServiceProvider.GetRequiredService<IPortfolioQueryRepository>();
        var queryItemsRepository = scope.ServiceProvider.GetRequiredService<IQueryRepository<PortfolioGalleryItemEntity, long>>();
        var portfolioEntity = await queryPortfolioRepository.GetByIdAsync(entity.Id);
        var entityItems = await queryItemsRepository.FindAsync(x => x.PorfolioId == entity.Id);
        portfolioEntity.ShouldBeNull();
        entityItems.ShouldBeEmpty();
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

    private async Task<PortfolioEntity> SetupEntityAsync()
    {
        await _testFixture.ResetDatabaseAsync();
        return await CreateAndPersistEntityAsync();
    }

    private async Task<PortfolioEntity> CreateAndPersistEntityAsync()
    {
        await using var scope = _testFixture.GetProvider().CreateAsyncScope();
        var commandRepository = scope.ServiceProvider.GetRequiredService<ICommandRepository<PortfolioEntity>>();

        var entity = _fixture.BuildPortfolioEntity();
        await commandRepository.CreateAsync(entity);

        return entity;
    }
}