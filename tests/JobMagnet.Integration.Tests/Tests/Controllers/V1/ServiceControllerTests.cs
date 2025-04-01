using System.Net;
using AutoFixture;
using FluentAssertions;
using JobMagnet.Infrastructure.Entities;
using JobMagnet.Infrastructure.Repositories.Base.Interfaces;
using JobMagnet.Infrastructure.Repositories.Interfaces;
using JobMagnet.Integration.Tests.Extensions;
using JobMagnet.Integration.Tests.Fixtures;
using JobMagnet.Integration.Tests.Utils;
using JobMagnet.Models.Service;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit.Abstractions;

namespace JobMagnet.Integration.Tests.Tests.Controllers.V1;

public class ServiceControllerTests : IClassFixture<JobMagnetTestSetupFixture>
{
    private const string RequestUriController = "api/v1/Service";
    private const string InvalidId = "100";
    private readonly IFixture _fixture = FixtureBuilder.Build();
    private readonly HttpClient _httpClient;
    private readonly JobMagnetTestSetupFixture _testFixture;

    public ServiceControllerTests(JobMagnetTestSetupFixture testFixture, ITestOutputHelper testOutputHelper)
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
        var createRequest = _fixture.Build<ServiceCreateRequest>().Create();
        var httpContent = TestUtilities.SerializeRequestContent(createRequest);

        // When
        var response = await _httpClient.PostAsync(RequestUriController, httpContent);

        // Then
        response.IsSuccessStatusCode.ShouldBeTrue();
        response.StatusCode.ShouldBe(HttpStatusCode.Created);

        var responseData = await TestUtilities.DeserializeResponseAsync<ServiceModel>(response);
        responseData.ShouldNotBeNull();

        var locationHeader = response.Headers.Location!.ToString();
        locationHeader.ShouldNotBeNull();
        locationHeader.ShouldContain($"{RequestUriController}/{responseData.Id}");

        await using var scope = _testFixture.GetProvider().CreateAsyncScope();
        var queryRepository = scope.ServiceProvider.GetRequiredService<IServiceQueryRepository>();
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

        var responseData = await TestUtilities.DeserializeResponseAsync<ServiceModel>(response);
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
        var queryServiceRepository = scope.ServiceProvider.GetRequiredService<IServiceQueryRepository>();
        var queryItemsRepository =
            scope.ServiceProvider.GetRequiredService<IQueryRepository<ServiceGalleryItemEntity, long>>();
        var serviceEntity = await queryServiceRepository.GetByIdAsync(entity.Id);
        var entityItems = await queryItemsRepository.FindAsync(x => x.ServiceId == entity.Id);
        serviceEntity.ShouldBeNull();
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
        var service = await SetupEntityAsync();
        var patchDocument = new JsonPatchDocument<ServiceRequest>();
        var itemAdded01 = _fixture.Create<ServiceGalleryItemRequest>();
        var itemAdded02 = _fixture.Create<ServiceGalleryItemRequest>();
        patchDocument.Add(p => p.GalleryItems, itemAdded01);
        patchDocument.Add(p => p.GalleryItems, itemAdded02);

        // When
        var response =
            await _httpClient.PatchAsNewtonsoftJsonAsync($"{RequestUriController}/{service.Id}", patchDocument);

        // Then
        response.IsSuccessStatusCode.ShouldBeTrue();
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);

        await using var scope = _testFixture.GetProvider().CreateAsyncScope();
        var queryServiceRepository = scope.ServiceProvider.GetRequiredService<IServiceQueryRepository>();
        _ = queryServiceRepository.IncludeGalleryItems();
        var serviceEntity = await queryServiceRepository.GetByIdWithIncludesAsync(service.Id);
        serviceEntity!.GalleryItems.Count.ShouldBe(service.GalleryItems.Count + patchDocument.Operations.Count);
        serviceEntity.GalleryItems.ShouldContain(x => x.Title == itemAdded01.Title);
        serviceEntity.GalleryItems.ShouldContain(x => x.Description == itemAdded01.Description);
        serviceEntity.GalleryItems.ShouldContain(x => x.UrlLink == itemAdded02.UrlLink);
        serviceEntity.GalleryItems.ShouldContain(x => x.UrlImage == itemAdded02.UrlImage);
        serviceEntity.GalleryItems.ShouldContain(x => x.UrlVideo == itemAdded02.UrlVideo);
        serviceEntity.GalleryItems.ShouldContain(x => x.Type == itemAdded02.Type);
        serviceEntity.GalleryItems.ShouldContain(x => x.Position == itemAdded02.Position);
    }

    [Fact(DisplayName = "Should handle Remove operations in a PATCH request")]
    public async Task ShouldHandleRemoveOperationsInPatchRequestAsync()
    {
        // Given
        var service = await SetupEntityAsync();
        var itemToRemove = service.GalleryItems.ElementAt(3);
        var indexItemToRemove = service.GalleryItems.ToList().FindIndex(item => item.Id == itemToRemove.Id);
        var patchDocument = new JsonPatchDocument<ServiceRequest>();
        patchDocument.Remove(p => p.GalleryItems, indexItemToRemove);

        // When
        var response =
            await _httpClient.PatchAsNewtonsoftJsonAsync($"{RequestUriController}/{service.Id}", patchDocument);

        // Then
        response.IsSuccessStatusCode.ShouldBeTrue();
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);

        await using var scope = _testFixture.GetProvider().CreateAsyncScope();
        var queryServiceRepository = scope.ServiceProvider.GetRequiredService<IServiceQueryRepository>();
        _ = queryServiceRepository.IncludeGalleryItems();
        var serviceEntity = await queryServiceRepository.GetByIdWithIncludesAsync(service.Id);
        serviceEntity!.GalleryItems.Count.ShouldBe(service.GalleryItems.Count - 1);
        serviceEntity.GalleryItems.Contains(itemToRemove).ShouldBeFalse();
    }

    [Fact(DisplayName = "Should handle Replace operations in a PATCH request")]
    public async Task ShouldHandleReplaceOperationsInPatchRequestAsync()
    {
        // Given
        var service = await SetupEntityAsync();
        var itemUpdated = _fixture.Create<ServiceGalleryItemRequest>();
        var itemToReplace = service.GalleryItems.ElementAt(3);
        itemUpdated.Id = itemToReplace.Id;
        var indexItemToReplace = service.GalleryItems.ToList().FindIndex(item => item.Id == itemToReplace.Id);
        var patchDocument = new JsonPatchDocument<ServiceRequest>();
        patchDocument.Replace(p => p.GalleryItems[indexItemToReplace], itemUpdated);

        // When
        var response =
            await _httpClient.PatchAsNewtonsoftJsonAsync($"{RequestUriController}/{service.Id}", patchDocument);

        // Then
        response.IsSuccessStatusCode.ShouldBeTrue();
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);

        await using var scope = _testFixture.GetProvider().CreateAsyncScope();
        var queryServiceRepository = scope.ServiceProvider.GetRequiredService<IServiceQueryRepository>();
        _ = queryServiceRepository.IncludeGalleryItems();
        var serviceEntity = await queryServiceRepository.GetByIdWithIncludesAsync(service.Id);
        serviceEntity!.GalleryItems.Count.ShouldBe(service.GalleryItems.Count);
        var entityUpdated = serviceEntity.GalleryItems.First(x => x.Id == itemUpdated.Id);
        entityUpdated.Should().BeEquivalentTo(itemUpdated, options => options
            .ExcludingMissingMembers()
            .Excluding(x => x.Id)
        );
    }

    [Fact(DisplayName = "Should handle multiple operations in a PATCH request")]
    public async Task ShouldHandleMultipleOperationsInPatchRequestAsync()
    {
        // Given
        var service = await SetupEntityAsync();
        var itemToReplace = service.GalleryItems.ElementAt(3);
        var itemToRemove = service.GalleryItems.ElementAt(1);
        var itemAdded01 = _fixture.Create<ServiceGalleryItemRequest>();
        var itemAdded02 = _fixture.Create<ServiceGalleryItemRequest>();
        var itemUpdated = _fixture.Create<ServiceGalleryItemRequest>();
        itemUpdated.Id = itemToReplace.Id;
        var indexItemToReplace = service.GalleryItems.ToList().FindIndex(item => item.Id == itemToReplace.Id);
        var indexItemToRemove = service.GalleryItems.ToList().FindIndex(item => item.Id == itemToRemove.Id);

        var patchDocument = new JsonPatchDocument<ServiceRequest>();
        patchDocument.Add(p => p.GalleryItems, itemAdded01);
        patchDocument.Add(p => p.GalleryItems, itemAdded02);
        patchDocument.Replace(p => p.GalleryItems[indexItemToReplace], itemUpdated);
        patchDocument.Remove(p => p.GalleryItems, indexItemToRemove);

        // When
        var response =
            await _httpClient.PatchAsNewtonsoftJsonAsync($"{RequestUriController}/{service.Id}", patchDocument);

        // Then
        response.IsSuccessStatusCode.ShouldBeTrue();
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);

        await using var scope = _testFixture.GetProvider().CreateAsyncScope();
        var queryServiceRepository = scope.ServiceProvider.GetRequiredService<IServiceQueryRepository>();
        _ = queryServiceRepository.IncludeGalleryItems();
        var serviceEntity = await queryServiceRepository.GetByIdWithIncludesAsync(service.Id);
        serviceEntity!.GalleryItems.Count.ShouldBe(service.GalleryItems.Count + 1);
        serviceEntity.GalleryItems.Should().ContainEquivalentOf(itemAdded01, options => options.ExcludingMissingMembers().Excluding(x => x.Id));
        serviceEntity.GalleryItems.Should().ContainEquivalentOf(itemAdded02, options => options.ExcludingMissingMembers().Excluding(x => x.Id));
        serviceEntity.GalleryItems.Should().ContainEquivalentOf(itemUpdated, options => options.ExcludingMissingMembers());
        serviceEntity.GalleryItems.Contains(itemToRemove).ShouldBeFalse();
    }

    private async Task<ServiceEntity> SetupEntityAsync()
    {
        await _testFixture.ResetDatabaseAsync();
        return await CreateAndPersistEntityAsync();
    }

    private async Task<ServiceEntity> CreateAndPersistEntityAsync()
    {
        await using var scope = _testFixture.GetProvider().CreateAsyncScope();
        var commandRepository = scope.ServiceProvider.GetRequiredService<ICommandRepository<ServiceEntity>>();

        var entity = _fixture.BuildServiceEntity();
        await commandRepository.CreateAsync(entity);

        return entity;
    }
}