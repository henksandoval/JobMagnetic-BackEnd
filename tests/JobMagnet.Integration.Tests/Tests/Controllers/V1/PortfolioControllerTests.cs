using System.Net;
using AutoFixture;
using FluentAssertions;
using JobMagnet.Infrastructure.Entities;
using JobMagnet.Infrastructure.Repositories.Base.Interfaces;
using JobMagnet.Infrastructure.Repositories.Interfaces;
using JobMagnet.Integration.Tests.Extensions;
using JobMagnet.Integration.Tests.Fixtures;
using JobMagnet.Shared.Tests.Utils;
using JobMagnet.Models.Portfolio;
using JobMagnet.Models.Resume;
using JobMagnet.Shared.Tests.Fixtures;
using JobMagnet.Shared.Tests.Fixtures.Builders;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit.Abstractions;

namespace JobMagnet.Integration.Tests.Tests.Controllers.V1;

public class PortfolioControllerTests : IClassFixture<JobMagnetTestSetupFixture>
{
    private const string RequestUriController = "api/v1/portfolio";
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
        var profileEntity = await SetupProfileEntityAsync();
        var createRequest = _fixture.Build<PortfolioCreateRequest>().With(x => x.ProfileId, profileEntity.Id).Create();
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
        entityCreated.GalleryItems.Should().BeEquivalentTo(createRequest.GalleryItems, options => options
            .ExcludingMissingMembers()
            .Excluding(x => x.Id)
        );
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
        var queryItemsRepository =
            scope.ServiceProvider.GetRequiredService<IQueryRepository<PortfolioGalleryItemEntity, long>>();
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

    [Fact(DisplayName = "Should handle multiple Add operations in a PATCH request")]
    public async Task ShouldHandleAddMultipleOperationsInPatchRequestAsync()
    {
        // Given
        var portfolio = await SetupEntityAsync();
        var patchDocument = new JsonPatchDocument<PortfolioRequest>();
        var itemAdded01 = _fixture.Create<PortfolioGalleryItemRequest>();
        var itemAdded02 = _fixture.Create<PortfolioGalleryItemRequest>();
        patchDocument.Add(p => p.GalleryItems, itemAdded01);
        patchDocument.Add(p => p.GalleryItems, itemAdded02);

        // When
        var response =
            await _httpClient.PatchAsNewtonsoftJsonAsync($"{RequestUriController}/{portfolio.Id}", patchDocument);

        // Then
        response.IsSuccessStatusCode.ShouldBeTrue();
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);

        await using var scope = _testFixture.GetProvider().CreateAsyncScope();
        var queryPortfolioRepository = scope.ServiceProvider.GetRequiredService<IPortfolioQueryRepository>();
        _ = queryPortfolioRepository.IncludeGalleryItems();
        var portfolioEntity = await queryPortfolioRepository.GetByIdWithIncludesAsync(portfolio.Id);
        portfolioEntity!.GalleryItems.Count.ShouldBe(portfolio.GalleryItems.Count + patchDocument.Operations.Count);
        portfolioEntity.GalleryItems.ShouldContain(x => x.Title == itemAdded01.Title);
        portfolioEntity.GalleryItems.ShouldContain(x => x.UrlImage == itemAdded01.UrlImage);
        portfolioEntity.GalleryItems.ShouldContain(x => x.Title == itemAdded02.Title);
        portfolioEntity.GalleryItems.ShouldContain(x => x.UrlImage == itemAdded02.UrlImage);
    }

    [Fact(DisplayName = "Should handle Remove operations in a PATCH request")]
    public async Task ShouldHandleRemoveOperationsInPatchRequestAsync()
    {
        // Given
        var portfolio = await SetupEntityAsync();
        var itemToRemove = portfolio.GalleryItems.ElementAt(0);
        var indexItemToRemove = portfolio.GalleryItems.ToList().FindIndex(item => item.Id == itemToRemove.Id);
        var patchDocument = new JsonPatchDocument<PortfolioRequest>();
        patchDocument.Remove(p => p.GalleryItems, indexItemToRemove);

        // When
        var response =
            await _httpClient.PatchAsNewtonsoftJsonAsync($"{RequestUriController}/{portfolio.Id}", patchDocument);

        // Then
        response.IsSuccessStatusCode.ShouldBeTrue();
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);

        await using var scope = _testFixture.GetProvider().CreateAsyncScope();
        var queryPortfolioRepository = scope.ServiceProvider.GetRequiredService<IPortfolioQueryRepository>();
        _ = queryPortfolioRepository.IncludeGalleryItems();
        var portfolioEntity = await queryPortfolioRepository.GetByIdWithIncludesAsync(portfolio.Id);
        portfolioEntity!.GalleryItems.Count.ShouldBe(portfolio.GalleryItems.Count - 1);
        portfolioEntity.GalleryItems.Contains(itemToRemove).ShouldBeFalse();
    }

    [Fact(DisplayName = "Should handle Replace operations in a PATCH request")]
    public async Task ShouldHandleReplaceOperationsInPatchRequestAsync()
    {
        // Given
        var portfolio = await SetupEntityAsync();
        var itemUpdated = _fixture.Create<PortfolioGalleryItemRequest>();
        var itemToReplace = portfolio.GalleryItems.ElementAt(1);
        itemUpdated.Id = itemToReplace.Id;
        var indexItemToReplace = portfolio.GalleryItems.ToList().FindIndex(item => item.Id == itemToReplace.Id);
        var patchDocument = new JsonPatchDocument<PortfolioRequest>();
        patchDocument.Replace(p => p.GalleryItems[indexItemToReplace], itemUpdated);

        // When
        var response =
            await _httpClient.PatchAsNewtonsoftJsonAsync($"{RequestUriController}/{portfolio.Id}", patchDocument);

        // Then
        response.IsSuccessStatusCode.ShouldBeTrue();
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);

        await using var scope = _testFixture.GetProvider().CreateAsyncScope();
        var queryPortfolioRepository = scope.ServiceProvider.GetRequiredService<IPortfolioQueryRepository>();
        _ = queryPortfolioRepository.IncludeGalleryItems();
        var portfolioEntity = await queryPortfolioRepository.GetByIdWithIncludesAsync(portfolio.Id);
        portfolioEntity!.GalleryItems.Count.ShouldBe(portfolio.GalleryItems.Count);
        var entityUpdated = portfolioEntity.GalleryItems.First(x => x.Id == itemUpdated.Id);
        entityUpdated.Should().BeEquivalentTo(itemUpdated, options => options
            .ExcludingMissingMembers()
            .Excluding(x => x.Id)
        );
    }

    [Fact(DisplayName = "Should handle multiple operations in a PATCH request")]
    public async Task ShouldHandleMultipleOperationsInPatchRequestAsync()
    {
        // Given
        var portfolio = await SetupEntityAsync();
        var itemToReplace = portfolio.GalleryItems.ElementAt(2);
        var itemToRemove = portfolio.GalleryItems.ElementAt(0);
        var itemAdded01 = _fixture.Create<PortfolioGalleryItemRequest>();
        var itemAdded02 = _fixture.Create<PortfolioGalleryItemRequest>();
        var itemUpdated = _fixture.Create<PortfolioGalleryItemRequest>();
        itemUpdated.Id = itemToReplace.Id;
        var indexItemToReplace = portfolio.GalleryItems.ToList().FindIndex(item => item.Id == itemToReplace.Id);
        var indexItemToRemove = portfolio.GalleryItems.ToList().FindIndex(item => item.Id == itemToRemove.Id);

        var patchDocument = new JsonPatchDocument<PortfolioRequest>();
        patchDocument.Add(p => p.GalleryItems, itemAdded01);
        patchDocument.Add(p => p.GalleryItems, itemAdded02);
        patchDocument.Replace(p => p.GalleryItems[indexItemToReplace], itemUpdated);
        patchDocument.Remove(p => p.GalleryItems, indexItemToRemove);

        // When
        var response =
            await _httpClient.PatchAsNewtonsoftJsonAsync($"{RequestUriController}/{portfolio.Id}", patchDocument);

        // Then
        response.IsSuccessStatusCode.ShouldBeTrue();
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);

        await using var scope = _testFixture.GetProvider().CreateAsyncScope();
        var queryPortfolioRepository = scope.ServiceProvider.GetRequiredService<IPortfolioQueryRepository>();
        _ = queryPortfolioRepository.IncludeGalleryItems();
        var portfolioEntity = await queryPortfolioRepository.GetByIdWithIncludesAsync(portfolio.Id);
        portfolioEntity!.GalleryItems.Count.ShouldBe(portfolio.GalleryItems.Count + 1);
        portfolioEntity.GalleryItems.ShouldContain(x => x.Title == itemAdded01.Title);
        portfolioEntity.GalleryItems.ShouldContain(x => x.UrlImage == itemAdded01.UrlImage);
        portfolioEntity.GalleryItems.ShouldContain(x => x.Title == itemAdded02.Title);
        portfolioEntity.GalleryItems.ShouldContain(x => x.UrlImage == itemAdded02.UrlImage);
        var entityUpdated = portfolioEntity.GalleryItems.First(x => x.Id == itemUpdated.Id);
        entityUpdated.Should().BeEquivalentTo(itemUpdated, options => options
            .ExcludingMissingMembers()
            .Excluding(x => x.Id)
        );
        portfolioEntity.GalleryItems.Contains(itemToRemove).ShouldBeFalse();
    }

    private async Task<ProfileEntity> SetupProfileEntityAsync()
    {
        await _testFixture.ResetDatabaseAsync();
        await using var scope = _testFixture.GetProvider().CreateAsyncScope();
        var commandRepository = scope.ServiceProvider.GetRequiredService<ICommandRepository<ProfileEntity>>();

        var entity = _fixture.Create<ProfileEntity>();
        await commandRepository.CreateAsync(entity);

        return entity;
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

        var entity = _fixture.Create<PortfolioEntity>();
        await commandRepository.CreateAsync(entity);

        return entity;
    }
}