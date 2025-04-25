using System.Net;
using AutoFixture;
using FluentAssertions;
using JobMagnet.Infrastructure.Entities;
using JobMagnet.Infrastructure.Repositories.Base.Interfaces;
using JobMagnet.Infrastructure.Repositories.Interfaces;
using JobMagnet.Integration.Tests.Extensions;
using JobMagnet.Integration.Tests.Fixtures;
using JobMagnet.Models.Skill;
using JobMagnet.Shared.Tests.Fixtures;
using JobMagnet.Shared.Tests.Utils;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit.Abstractions;

namespace JobMagnet.Integration.Tests.Tests.Controllers.V1;

public class SkillControllerShould : IClassFixture<JobMagnetTestSetupFixture>
{
    private const string RequestUriController = "api/v1/skill";
    private const string InvalidId = "100";
    private readonly IFixture _fixture = FixtureBuilder.Build();
    private readonly HttpClient _httpClient;
    private readonly JobMagnetTestSetupFixture _testFixture;

    public SkillControllerShould(JobMagnetTestSetupFixture testFixture, ITestOutputHelper testOutputHelper)
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
        var createRequest = _fixture.Build<SkillCreateRequest>().With(x => x.ProfileId, profileEntity.Id).Create();
        var httpContent = TestUtilities.SerializeRequestContent(createRequest);

        // When
        var response = await _httpClient.PostAsync(RequestUriController, httpContent);

        // Then
        response.IsSuccessStatusCode.ShouldBeTrue();
        response.StatusCode.ShouldBe(HttpStatusCode.Created);

        var responseData = await TestUtilities.DeserializeResponseAsync<SkillModel>(response);
        responseData.ShouldNotBeNull();

        var locationHeader = response.Headers.Location!.ToString();
        locationHeader.ShouldNotBeNull();
        locationHeader.ShouldContain($"{RequestUriController}/{responseData.Id}");

        await using var scope = _testFixture.GetProvider().CreateAsyncScope();
        var queryRepository = scope.ServiceProvider.GetRequiredService<ISkillQueryRepository>();
        _ = queryRepository.IncludeDetails();
        var entityCreated = await queryRepository.GetByIdWithIncludesAsync(responseData.Id);

        entityCreated.ShouldNotBeNull();
        entityCreated.SkillDetails.Should().BeEquivalentTo(createRequest.SkillDetails, options => options
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

        var responseData = await TestUtilities.DeserializeResponseAsync<SkillModel>(response);
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
        var querySkillRepository = scope.ServiceProvider.GetRequiredService<ISkillQueryRepository>();
        var queryItemsRepository = scope.ServiceProvider.GetRequiredService<IQueryRepository<SkillItemEntity, long>>();
        var skillEntity = await querySkillRepository.GetByIdAsync(entity.Id);
        var entityItems = await queryItemsRepository.FindAsync(x => x.SkillId == entity.Id);
        skillEntity.ShouldBeNull();
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
        var skill = await SetupEntityAsync();
        var patchDocument = new JsonPatchDocument<SkillRequest>();
        var itemAdded01 = _fixture.Create<SkillItemRequest>();
        var itemAdded02 = _fixture.Create<SkillItemRequest>();
        patchDocument.Add(p => p.SkillDetails, itemAdded01);
        patchDocument.Add(p => p.SkillDetails, itemAdded02);

        // When
        var response =
            await _httpClient.PatchAsNewtonsoftJsonAsync($"{RequestUriController}/{skill.Id}", patchDocument);

        // Then
        response.IsSuccessStatusCode.ShouldBeTrue();
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);

        await using var scope = _testFixture.GetProvider().CreateAsyncScope();
        var querySkillRepository = scope.ServiceProvider.GetRequiredService<ISkillQueryRepository>();
        _ = querySkillRepository.IncludeDetails();
        var skillEntity = await querySkillRepository.GetByIdWithIncludesAsync(skill.Id);
        skillEntity!.SkillDetails.Count.ShouldBe(skill.SkillDetails.Count + patchDocument.Operations.Count);
        skillEntity.SkillDetails.ShouldContain(x => x.Name == itemAdded01.Name);
        skillEntity.SkillDetails.ShouldContain(x => x.ProficiencyLevel == itemAdded01.ProficiencyLevel);
        skillEntity.SkillDetails.ShouldContain(x => x.Category == itemAdded02.Category);
        skillEntity.SkillDetails.ShouldContain(x => x.Rank == itemAdded02.Rank);
        skillEntity.SkillDetails.ShouldContain(x => x.IconUrl == itemAdded02.IconUrl);
    }

    [Fact(DisplayName = "Handle Remove operations in a PATCH request")]
    public async Task HandleRemoveOperationsInPatchRequestAsync()
    {
        // Given
        var skill = await SetupEntityAsync();
        var itemToRemove = skill.SkillDetails.ElementAt(2);
        var indexItemToRemove = skill.SkillDetails.ToList().FindIndex(item => item.Id == itemToRemove.Id);
        var patchDocument = new JsonPatchDocument<SkillRequest>();
        patchDocument.Remove(p => p.SkillDetails, indexItemToRemove);

        // When
        var response =
            await _httpClient.PatchAsNewtonsoftJsonAsync($"{RequestUriController}/{skill.Id}", patchDocument);

        // Then
        response.IsSuccessStatusCode.ShouldBeTrue();
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);

        await using var scope = _testFixture.GetProvider().CreateAsyncScope();
        var querySkillRepository = scope.ServiceProvider.GetRequiredService<ISkillQueryRepository>();
        _ = querySkillRepository.IncludeDetails();
        var skillEntity = await querySkillRepository.GetByIdWithIncludesAsync(skill.Id);
        skillEntity!.SkillDetails.Count.ShouldBe(skill.SkillDetails.Count - 1);
        skillEntity.SkillDetails.Contains(itemToRemove).ShouldBeFalse();
    }

    [Fact(DisplayName = "Handle Replace operations in a PATCH request")]
    public async Task HandleReplaceOperationsInPatchRequestAsync()
    {
        // Given
        var skill = await SetupEntityAsync();
        var itemUpdated = _fixture.Create<SkillItemRequest>();
        var itemToReplace = skill.SkillDetails.ElementAt(0);
        itemUpdated.Id = itemToReplace.Id;
        var indexItemToReplace = skill.SkillDetails.ToList().FindIndex(item => item.Id == itemToReplace.Id);
        var patchDocument = new JsonPatchDocument<SkillRequest>();
        patchDocument.Replace(p => p.SkillDetails[indexItemToReplace], itemUpdated);

        // When
        var response =
            await _httpClient.PatchAsNewtonsoftJsonAsync($"{RequestUriController}/{skill.Id}", patchDocument);

        // Then
        response.IsSuccessStatusCode.ShouldBeTrue();
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);

        await using var scope = _testFixture.GetProvider().CreateAsyncScope();
        var querySkillRepository = scope.ServiceProvider.GetRequiredService<ISkillQueryRepository>();
        _ = querySkillRepository.IncludeDetails();
        var skillEntity = await querySkillRepository.GetByIdWithIncludesAsync(skill.Id);
        skillEntity!.SkillDetails.Count.ShouldBe(skill.SkillDetails.Count);
        var entityUpdated = skillEntity.SkillDetails.First(x => x.Id == itemUpdated.Id);
        entityUpdated.Should().BeEquivalentTo(itemUpdated, options => options
            .ExcludingMissingMembers()
            .Excluding(x => x.Id)
        );
    }

    [Fact(DisplayName = "Handle multiple operations in a PATCH request")]
    public async Task HandleMultipleOperationsInPatchRequestAsync()
    {
        // Given
        var skill = await SetupEntityAsync();
        var itemToReplace = skill.SkillDetails.ElementAt(2);
        var itemToRemove = skill.SkillDetails.ElementAt(0);
        var itemAdded01 = _fixture.Create<SkillItemRequest>();
        var itemAdded02 = _fixture.Create<SkillItemRequest>();
        var itemUpdated = _fixture.Create<SkillItemRequest>();
        itemUpdated.Id = itemToReplace.Id;
        var indexItemToReplace = skill.SkillDetails.ToList().FindIndex(item => item.Id == itemToReplace.Id);
        var indexItemToRemove = skill.SkillDetails.ToList().FindIndex(item => item.Id == itemToRemove.Id);

        var patchDocument = new JsonPatchDocument<SkillRequest>();
        patchDocument.Add(p => p.SkillDetails, itemAdded01);
        patchDocument.Add(p => p.SkillDetails, itemAdded02);
        patchDocument.Replace(p => p.SkillDetails[indexItemToReplace], itemUpdated);
        patchDocument.Remove(p => p.SkillDetails, indexItemToRemove);

        // When
        var response =
            await _httpClient.PatchAsNewtonsoftJsonAsync($"{RequestUriController}/{skill.Id}", patchDocument);

        // Then
        response.IsSuccessStatusCode.ShouldBeTrue();
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);

        await using var scope = _testFixture.GetProvider().CreateAsyncScope();
        var querySkillRepository = scope.ServiceProvider.GetRequiredService<ISkillQueryRepository>();
        _ = querySkillRepository.IncludeDetails();
        var skillEntity = await querySkillRepository.GetByIdWithIncludesAsync(skill.Id);
        skillEntity!.SkillDetails.Count.ShouldBe(skill.SkillDetails.Count + 1);
        skillEntity.SkillDetails.ShouldContain(x => x.Name == itemAdded01.Name);
        skillEntity.SkillDetails.ShouldContain(x => x.ProficiencyLevel == itemAdded01.ProficiencyLevel);
        skillEntity.SkillDetails.ShouldContain(x => x.Category == itemAdded02.Category);
        skillEntity.SkillDetails.ShouldContain(x => x.Rank == itemAdded02.Rank);
        skillEntity.SkillDetails.ShouldContain(x => x.IconUrl == itemAdded02.IconUrl);
        var entityUpdated = skillEntity.SkillDetails.First(x => x.Id == itemUpdated.Id);
        entityUpdated.Should().BeEquivalentTo(itemUpdated, options => options
            .ExcludingMissingMembers()
            .Excluding(x => x.Id)
        );
        skillEntity.SkillDetails.Contains(itemToRemove).ShouldBeFalse();
    }

    private async Task<SkillEntity> SetupEntityAsync()
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

    private async Task<SkillEntity> CreateAndPersistEntityAsync()
    {
        await using var scope = _testFixture.GetProvider().CreateAsyncScope();
        var commandRepository = scope.ServiceProvider.GetRequiredService<ICommandRepository<SkillEntity>>();

        var entity = _fixture.Create<SkillEntity>();
        await commandRepository.CreateAsync(entity);

        return entity;
    }
}