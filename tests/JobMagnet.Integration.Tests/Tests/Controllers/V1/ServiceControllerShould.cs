using System.Net;
using AutoFixture;
using FluentAssertions;
using JobMagnet.Application.Commands.Service;
using JobMagnet.Application.Models.Base;
using JobMagnet.Application.Models.Responses.Service;
using JobMagnet.Domain.Core.Entities;
using JobMagnet.Domain.Ports.Repositories;
using JobMagnet.Domain.Ports.Repositories.Base;
using JobMagnet.Integration.Tests.Extensions;
using JobMagnet.Integration.Tests.Fixtures;
using JobMagnet.Shared.Tests.Fixtures;
using JobMagnet.Shared.Tests.Utils;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit.Abstractions;

namespace JobMagnet.Integration.Tests.Tests.Controllers.V1;

public class ServiceControllerShould : IClassFixture<JobMagnetTestSetupFixture>
{
    private const string RequestUriController = "api/v1/Service";
    private const string InvalidId = "100";
    private readonly IFixture _fixture = FixtureBuilder.Build();
    private readonly HttpClient _httpClient;
    private readonly JobMagnetTestSetupFixture _testFixture;

    public ServiceControllerShould(JobMagnetTestSetupFixture testFixture, ITestOutputHelper testOutputHelper)
    {
        _testFixture = testFixture;
        _httpClient = _testFixture.GetClient();
        _testFixture.SetTestOutputHelper(testOutputHelper);
    }

    [Fact(DisplayName = "Create a new record and return 201 when the POST request is valid")]
    public async Task ReturnCreatedAndPersistData_WhenRequestIsValidAsync()
    {
        // Given
        await _testFixture.ResetDatabaseAsync();
        var profileEntity = await SetupProfileEntityAsync();
        var serviceData = _fixture.Build<ServiceBase>()
            .With(x => x.ProfileId, profileEntity.Id)
            .Create();
        var createRequest = _fixture.Build<ServiceCommand>().With(x => x.ServiceData, serviceData).Create();
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
        entityCreated.GalleryItems.Should().BeEquivalentTo(createRequest.ServiceData.GalleryItems, options => options
            .ExcludingMissingMembers()
            .Excluding(x => x.Id)
        );
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

        var responseData = await TestUtilities.DeserializeResponseAsync<ServiceModel>(response);
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

    [Fact(DisplayName = "Delete and return 204 when DELETE request is received")]
    public async Task DeleteRecord_WhenDeleteRequestIsReceivedAsync()
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

    [Fact(DisplayName = "Handle multiple Add operations in a PATCH request")]
    public async Task HandleAddMultipleOperationsInPatchRequestAsync()
    {
        // Given
        var service = await SetupEntityAsync();
        var patchDocument = new JsonPatchDocument<ServiceCommand>();
        var itemAdded01 = _fixture.Create<ServiceGalleryItemBase>();
        var itemAdded02 = _fixture.Create<ServiceGalleryItemBase>();
        patchDocument.Add(p => p.ServiceData.GalleryItems, itemAdded01);
        patchDocument.Add(p => p.ServiceData.GalleryItems, itemAdded02);

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

    [Fact(DisplayName = "Handle Remove operations in a PATCH request")]
    public async Task HandleRemoveOperationsInPatchRequestAsync()
    {
        // Given
        var service = await SetupEntityAsync();
        var itemToRemove = service.GalleryItems.ElementAt(2);
        var indexItemToRemove = service.GalleryItems.ToList().FindIndex(item => item.Id == itemToRemove.Id);
        var patchDocument = new JsonPatchDocument<ServiceCommand>();
        patchDocument.Remove(p => p.ServiceData.GalleryItems, indexItemToRemove);

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

    [Fact(DisplayName = "Handle Replace operations in a PATCH request")]
    public async Task HandleReplaceOperationsInPatchRequestAsync()
    {
        // Given
        var service = await SetupEntityAsync();
        var itemToReplace = service.GalleryItems.ElementAt(2);
        var itemUpdated = _fixture.Build<ServiceGalleryItemBase>().With(s => s.Id, itemToReplace.Id).Create();
        var indexItemToReplace = service.GalleryItems.ToList().FindIndex(item => item.Id == itemToReplace.Id);
        var patchDocument = new JsonPatchDocument<ServiceCommand>();
        patchDocument.Replace(p => p.ServiceData.GalleryItems[indexItemToReplace], itemUpdated);

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

    [Fact(DisplayName = "Handle multiple operations in a PATCH request")]
    public async Task HandleMultipleOperationsInPatchRequestAsync()
    {
        // Given
        var service = await SetupEntityAsync();
        var itemToReplace = service.GalleryItems.ElementAt(2);
        var itemToRemove = service.GalleryItems.ElementAt(0);

        var itemAdded01 = _fixture.Create<ServiceGalleryItemBase>();
        var itemAdded02 = _fixture.Create<ServiceGalleryItemBase>();
        var itemUpdated = _fixture.Build<ServiceGalleryItemBase>().With(s => s.Id, itemToReplace.Id).Create();

        var indexItemToReplace = service.GalleryItems.ToList().FindIndex(item => item.Id == itemToReplace.Id);
        var indexItemToRemove = service.GalleryItems.ToList().FindIndex(item => item.Id == itemToRemove.Id);

        var patchDocument = new JsonPatchDocument<ServiceCommand>();
        patchDocument.Add(p => p.ServiceData.GalleryItems, itemAdded01);
        patchDocument.Add(p => p.ServiceData.GalleryItems, itemAdded02);
        patchDocument.Replace(p => p.ServiceData.GalleryItems[indexItemToReplace], itemUpdated);
        patchDocument.Remove(p => p.ServiceData.GalleryItems, indexItemToRemove);

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
        serviceEntity.GalleryItems.Should().ContainEquivalentOf(itemAdded01,
            options => options.ExcludingMissingMembers().Excluding(x => x.Id));
        serviceEntity.GalleryItems.Should().ContainEquivalentOf(itemAdded02,
            options => options.ExcludingMissingMembers().Excluding(x => x.Id));
        serviceEntity.GalleryItems.Should()
            .ContainEquivalentOf(itemUpdated, options => options.ExcludingMissingMembers());
        serviceEntity.GalleryItems.Contains(itemToRemove).ShouldBeFalse();
    }

    private async Task<ServiceEntity> SetupEntityAsync()
    {
        await _testFixture.ResetDatabaseAsync();
        return await CreateAndPersistEntityAsync();
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

    private async Task<ServiceEntity> CreateAndPersistEntityAsync()
    {
        await using var scope = _testFixture.GetProvider().CreateAsyncScope();
        var commandRepository = scope.ServiceProvider.GetRequiredService<ICommandRepository<ServiceEntity>>();

        var entity = _fixture.Create<ServiceEntity>();
        await commandRepository.CreateAsync(entity);

        return entity;
    }
}