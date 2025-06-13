using System.Net;
using AutoFixture;
using FluentAssertions;
using JobMagnet.Application.Contracts.Commands.Skill;
using JobMagnet.Application.Contracts.Responses.Base;
using JobMagnet.Application.Contracts.Responses.Skill;
using JobMagnet.Domain.Core.Entities;
using JobMagnet.Domain.Ports.Repositories;
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
        var skillData = _fixture.Build<SkillBase>()
            .With(x => x.ProfileId, profileEntity.Id)
            .Create();
        var createRequest = _fixture.Build<SkillCommand>().With(x => x.SkillData, skillData).Create();
        var httpContent = TestUtilities.SerializeRequestContent(createRequest);

        // When
        var response = await _httpClient.PostAsync(RequestUriController, httpContent);

        // Then
        response.IsSuccessStatusCode.ShouldBeTrue();
        response.StatusCode.ShouldBe(HttpStatusCode.Created);

        var responseData = await TestUtilities.DeserializeResponseAsync<SkillResponse>(response);
        responseData.ShouldNotBeNull();

        var locationHeader = response.Headers.Location!.ToString();
        locationHeader.ShouldNotBeNull();
        locationHeader.ShouldContain($"{RequestUriController}/{responseData.Id}");

        await using var scope = _testFixture.GetProvider().CreateAsyncScope();
        var queryRepository = scope.ServiceProvider.GetRequiredService<ISkillQueryRepository>();
        _ = queryRepository.IncludeDetails();
        var entityCreated = await queryRepository.GetByIdWithIncludesAsync(responseData.Id);

        entityCreated.ShouldNotBeNull();
        entityCreated.Skills.Should().BeEquivalentTo(createRequest.SkillData.SkillDetails, options => options
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

        var responseData = await TestUtilities.DeserializeResponseAsync<SkillResponse>(response);
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
        var queryItemsRepository = scope.ServiceProvider.GetRequiredService<IQueryRepository<SkillEntity, long>>();
        var skillEntity = await querySkillRepository.GetByIdAsync(entity.Id, CancellationToken.None);
        var entityItems = await queryItemsRepository.FindAsync(x => x.SkillId == entity.Id, CancellationToken.None);
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
        var patchDocument = new JsonPatchDocument<SkillCommand>();
        var itemAdded01 = _fixture.Create<SkillItemBase>();
        var itemAdded02 = _fixture.Create<SkillItemBase>();
        patchDocument.Add(p => p.SkillData.SkillDetails, itemAdded01);
        patchDocument.Add(p => p.SkillData.SkillDetails, itemAdded02);

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
        skillEntity!.Skills.Count.ShouldBe(skill.Skills.Count + patchDocument.Operations.Count);
        skillEntity.Skills.ShouldContain(x => x.Name == itemAdded01.Name);
        skillEntity.Skills.ShouldContain(x => x.ProficiencyLevel == itemAdded01.ProficiencyLevel);
        skillEntity.Skills.ShouldContain(x => x.Category == itemAdded02.Category);
        skillEntity.Skills.ShouldContain(x => x.Rank == itemAdded02.Rank);
        skillEntity.Skills.ShouldContain(x => x.IconUrl == itemAdded02.IconUrl);
    }

    [Fact(DisplayName = "Handle Remove operations in a PATCH request")]
    public async Task HandleRemoveOperationsInPatchRequestAsync()
    {
        // Given
        var skill = await SetupEntityAsync();
        var itemToRemove = skill.Skills.ElementAt(2);
        var indexItemToRemove = skill.Skills.ToList().FindIndex(item => item.Id == itemToRemove.Id);
        var patchDocument = new JsonPatchDocument<SkillCommand>();
        patchDocument.Remove(p => p.SkillData.SkillDetails, indexItemToRemove);

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
        skillEntity!.Skills.Count.ShouldBe(skill.Skills.Count - 1);
        skillEntity.Skills.Contains(itemToRemove).ShouldBeFalse();
    }

    [Fact(DisplayName = "Handle Replace operations in a PATCH request")]
    public async Task HandleReplaceOperationsInPatchRequestAsync()
    {
        // Given
        var skill = await SetupEntityAsync();
        var itemToReplace = skill.Skills.ElementAt(0);
        var itemUpdated = _fixture.Build<SkillItemBase>().With(s => s.Id, itemToReplace.Id).Create();
        var indexItemToReplace = skill.Skills.ToList().FindIndex(item => item.Id == itemToReplace.Id);
        var patchDocument = new JsonPatchDocument<SkillCommand>();
        patchDocument.Replace(p => p.SkillData.SkillDetails[indexItemToReplace], itemUpdated);

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
        skillEntity!.Skills.Count.ShouldBe(skill.Skills.Count);
        var entityUpdated = skillEntity.Skills.First(x => x.Id == itemUpdated.Id);
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
        var itemToRemove = skill.Skills.ElementAt(0);
        var itemAdded01 = _fixture.Create<SkillItemBase>();
        var itemAdded02 = _fixture.Create<SkillItemBase>();
        var itemToReplace = skill.Skills.ElementAt(2);
        var itemUpdated = _fixture.Build<SkillItemBase>().With(s => s.Id, itemToReplace.Id).Create();
        var indexItemToReplace = skill.Skills.ToList().FindIndex(item => item.Id == itemToReplace.Id);
        var indexItemToRemove = skill.Skills.ToList().FindIndex(item => item.Id == itemToRemove.Id);

        var patchDocument = new JsonPatchDocument<SkillCommand>();
        patchDocument.Add(p => p.SkillData.SkillDetails, itemAdded01);
        patchDocument.Add(p => p.SkillData.SkillDetails, itemAdded02);
        patchDocument.Replace(p => p.SkillData.SkillDetails[indexItemToReplace], itemUpdated);
        patchDocument.Remove(p => p.SkillData.SkillDetails, indexItemToRemove);

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
        skillEntity!.Skills.Count.ShouldBe(skill.Skills.Count + 1);
        skillEntity.Skills.ShouldContain(x => x.Name == itemAdded01.Name);
        skillEntity.Skills.ShouldContain(x => x.ProficiencyLevel == itemAdded01.ProficiencyLevel);
        skillEntity.Skills.ShouldContain(x => x.Category == itemAdded02.Category);
        skillEntity.Skills.ShouldContain(x => x.Rank == itemAdded02.Rank);
        skillEntity.Skills.ShouldContain(x => x.IconUrl == itemAdded02.IconUrl);
        var entityUpdated = skillEntity.Skills.First(x => x.Id == itemUpdated.Id);
        entityUpdated.Should().BeEquivalentTo(itemUpdated, options => options
            .ExcludingMissingMembers()
            .Excluding(x => x.Id)
        );
        skillEntity.Skills.Contains(itemToRemove).ShouldBeFalse();
    }

    private async Task<SkillSetEntity> SetupEntityAsync()
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

    private async Task<SkillSetEntity> CreateAndPersistEntityAsync()
    {
        await using var scope = _testFixture.GetProvider().CreateAsyncScope();
        var commandRepository = scope.ServiceProvider.GetRequiredService<ICommandRepository<SkillSetEntity>>();

        var entity = _fixture.Create<SkillSetEntity>();
        await commandRepository.CreateAsync(entity);
        await commandRepository.SaveChangesAsync();

        return entity;
    }
}