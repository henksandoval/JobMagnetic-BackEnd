using System.Net;
using AutoFixture;
using FluentAssertions;
using JobMagnet.Infrastructure.Entities;
using JobMagnet.Infrastructure.Repositories.Base.Interfaces;
using JobMagnet.Infrastructure.Repositories.Interfaces;
using JobMagnet.Integration.Tests.Extensions;
using JobMagnet.Integration.Tests.Fixtures;
using JobMagnet.Models.Summary;
using JobMagnet.Models.Summary.Education;
using JobMagnet.Models.Summary.WorkExperience;
using JobMagnet.Shared.Tests.Fixtures;
using JobMagnet.Shared.Tests.Fixtures.Builders;
using JobMagnet.Shared.Tests.Utils;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit.Abstractions;

namespace JobMagnet.Integration.Tests.Tests.Controllers.V1;

public class SummaryControllerTests : IClassFixture<JobMagnetTestSetupFixture>
{
    private const string RequestUriController = "api/v1/Summary";
    private const string InvalidId = "100";
    private readonly IFixture _fixture = FixtureBuilder.Build();
    private readonly HttpClient _httpClient;
    private readonly JobMagnetTestSetupFixture _testFixture;

    public SummaryControllerTests(JobMagnetTestSetupFixture testFixture, ITestOutputHelper testOutputHelper)
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
        var createRequest = _fixture.Build<SummaryCreateRequest>().With(x => x.ProfileId, profileEntity.Id).Create();
        var httpContent = TestUtilities.SerializeRequestContent(createRequest);

        // When
        var response = await _httpClient.PostAsync(RequestUriController, httpContent);

        // Then
        response.IsSuccessStatusCode.ShouldBeTrue();
        response.StatusCode.ShouldBe(HttpStatusCode.Created);

        var responseData = await TestUtilities.DeserializeResponseAsync<SummaryModel>(response);
        responseData.ShouldNotBeNull();

        var locationHeader = response.Headers.Location!.ToString();
        locationHeader.ShouldNotBeNull();
        locationHeader.ShouldContain($"{RequestUriController}/{responseData.Id}");

        await using var scope = _testFixture.GetProvider().CreateAsyncScope();
        var queryRepository = scope.ServiceProvider.GetRequiredService<ISummaryQueryRepository>();
        var entityCreated = await queryRepository.GetByIdWithIncludesAsync(responseData.Id);

        entityCreated.ShouldNotBeNull();
    }

    [Fact(DisplayName = "Should return the record and return 200 when GET request with valid ID is provided")]
    public async Task ShouldReturnRecord_WhenValidIdIsProvidedAsync()
    {
        // Given
        var entity = await SetupEntityAsync(() => _fixture.BuildSummaryEntity());

        // When
        var response = await _httpClient.GetAsync($"{RequestUriController}/{entity.Id}");

        // Then
        response.IsSuccessStatusCode.ShouldBeTrue();
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var responseData = await TestUtilities.DeserializeResponseAsync<SummaryModel>(response);
        responseData.ShouldNotBeNull();
        responseData.Should().BeEquivalentTo(entity, options => options.ExcludingMissingMembers());
    }

    [Fact(DisplayName = "Should return 404 when GET request with invalid ID is provided")]
    public async Task ShouldReturnNotFound_WhenInvalidIdIsProvidedAsync()
    {
        // Given
        _ = await SetupEntityAsync(() => _fixture.BuildSummaryEntity());

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
        var entity = await SetupEntityAsync(() => _fixture.BuildSummaryEntity());

        // When
        var response = await _httpClient.DeleteAsync($"{RequestUriController}/{entity.Id}");

        // Then
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);

        await using var scope = _testFixture.GetProvider().CreateAsyncScope();
        var querySummaryRepository = scope.ServiceProvider.GetRequiredService<ISummaryQueryRepository>();
        var summaryEntity = await querySummaryRepository.GetByIdAsync(entity.Id);
        summaryEntity.ShouldBeNull();
    }

    [Fact(DisplayName = "Should return 404 when a DELETE request with invalid ID is provided")]
    public async Task ShouldReturnNotFound_WhenDeleteRequestWithInvalidIdIsProvidedAsync()
    {
        // Given
        _ = await SetupEntityAsync(() => _fixture.BuildSummaryEntity());

        // When
        var response = await _httpClient.DeleteAsync($"{RequestUriController}/{InvalidId}");

        // Then
        response.IsSuccessStatusCode.ShouldBeFalse();
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    [Fact(DisplayName = "Should handle multiple Education Add operations in a PATCH request")]
    public async Task ShouldHandleAddMultipleEducationOperationsInPatchEducationRequestAsync()
    {
        // Given
        var summary = await SetupEntityAsync(() => _fixture.BuildSummaryEntity());
        var itemAdded01 = _fixture.Build<EducationRequest>().Without(x => x.Id).Create();
        var itemAdded02 = _fixture.Build<EducationRequest>().Without(x => x.Id).Create();
        var patchDocument = new JsonPatchDocument<SummaryComplexRequest>();
        patchDocument.Add(x => x.Education, itemAdded01);
        patchDocument.Add(x => x.Education, itemAdded02);

        // When
        var response =
            await _httpClient.PatchAsNewtonsoftJsonAsync($"{RequestUriController}/{summary.Id}/education",
                patchDocument);

        // Then
        response.IsSuccessStatusCode.ShouldBeTrue();
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);

        await using var scope = _testFixture.GetProvider().CreateAsyncScope();
        var querySkillRepository = scope.ServiceProvider.GetRequiredService<ISummaryQueryRepository>();
        _ = querySkillRepository.IncludeEducation();
        var summaryEntity = await querySkillRepository.GetByIdWithIncludesAsync(summary.Id);
        summaryEntity.ShouldNotBeNull();
        summaryEntity.Education.Count.ShouldBe(2);
        summaryEntity.Education.Should().BeEquivalentTo(new List<EducationRequest> { itemAdded01, itemAdded02 },
            options => options.ExcludingMissingMembers().Excluding(x => x.Id));
    }

    [Fact(DisplayName = "Should handle multiple Education operations in a PATCH request")]
    public async Task ShouldHandleMultipleEducationOperationsInPatchEducationRequestAsync()
    {
        // Given
        var itemAdded01 = _fixture.Build<EducationRequest>().Without(x => x.Id).Create();
        var itemAdded02 = _fixture.Build<EducationRequest>().Without(x => x.Id).Create();
        var itemUpdated = _fixture.Build<EducationRequest>().Without(x => x.Id).Create();

        var initialSummaryEntity = await SetupEntityAsync(() => _fixture.BuildSummaryEntityWithRelations());
        var itemToReplace = initialSummaryEntity.Education.ElementAt(3);
        var itemToRemove = initialSummaryEntity.Education.ElementAt(1);
        itemUpdated.Id = itemToReplace.Id;
        var indexItemToReplace = initialSummaryEntity.Education.ToList().FindIndex(item => item.Id == itemToReplace.Id);
        var indexItemToRemove = initialSummaryEntity.Education.ToList().FindIndex(item => item.Id == itemToRemove.Id);

        var patchDocument = new JsonPatchDocument<SummaryComplexRequest>();
        patchDocument.Add(x => x.Education, itemAdded01);
        patchDocument.Add(x => x.Education, itemAdded02);
        patchDocument.Replace(p => p.Education[indexItemToReplace], itemUpdated);
        patchDocument.Remove(p => p.Education, indexItemToRemove);

        // When
        var response =
            await _httpClient.PatchAsNewtonsoftJsonAsync($"{RequestUriController}/{initialSummaryEntity.Id}/education",
                patchDocument);

        // Then
        response.IsSuccessStatusCode.ShouldBeTrue();
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);

        await using var scope = _testFixture.GetProvider().CreateAsyncScope();
        var querySkillRepository = scope.ServiceProvider.GetRequiredService<ISummaryQueryRepository>();
        _ = querySkillRepository.IncludeEducation();
        var summaryEntity = await querySkillRepository.GetByIdWithIncludesAsync(initialSummaryEntity.Id);
        summaryEntity.ShouldNotBeNull();
        summaryEntity.Education.Count.ShouldBe(initialSummaryEntity.Education.Count + 1);
        summaryEntity.Education.Should().ContainEquivalentOf(itemAdded01,
            options => options.ExcludingMissingMembers().Excluding(x => x.Id));
        summaryEntity.Education.Should().ContainEquivalentOf(itemAdded02,
            options => options.ExcludingMissingMembers().Excluding(x => x.Id));
        summaryEntity.Education.Should().ContainEquivalentOf(itemUpdated,
            options => options.ExcludingMissingMembers());
        summaryEntity.Education.Contains(itemToRemove).ShouldBeFalse();
    }

    [Fact(DisplayName = "Should handle multiple Work Experience Add operations in a PATCH request")]
    public async Task ShouldHandleAddMultipleWorkExperienceOperationsInPatchWorkExperienceRequestAsync()
    {
        // Given
        var summary = await SetupEntityAsync(() => _fixture.BuildSummaryEntity());
        var itemAdded01 = _fixture.Build<WorkExperienceRequest>().Without(x => x.Id).Create();
        var itemAdded02 = _fixture.Build<WorkExperienceRequest>().Without(x => x.Id).Create();
        var patchDocument = new JsonPatchDocument<SummaryComplexRequest>();
        patchDocument.Add(x => x.WorkExperiences, itemAdded01);
        patchDocument.Add(x => x.WorkExperiences, itemAdded02);

        // When
        var response =
            await _httpClient.PatchAsNewtonsoftJsonAsync($"{RequestUriController}/{summary.Id}/WorkExperience",
                patchDocument);

        // Then
        response.IsSuccessStatusCode.ShouldBeTrue();
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);

        await using var scope = _testFixture.GetProvider().CreateAsyncScope();
        var querySkillRepository = scope.ServiceProvider.GetRequiredService<ISummaryQueryRepository>();
        _ = querySkillRepository.IncludeWorkExperience();
        var summaryEntity = await querySkillRepository.GetByIdWithIncludesAsync(summary.Id);
        summaryEntity.ShouldNotBeNull();
        summaryEntity.WorkExperiences.Count.ShouldBe(2);
        summaryEntity.WorkExperiences.Should().BeEquivalentTo(
            new List<WorkExperienceRequest> { itemAdded01, itemAdded02 },
            options => options.ExcludingMissingMembers().Excluding(x => x.Id));
    }

    [Fact(DisplayName = "Should handle multiple Work Experience in a PATCH request")]
    public async Task ShouldHandleMultipleWorkExperienceOperationsInPatchRequestAsync()
    {
        // Given
        var itemAdded01 = _fixture.Build<WorkExperienceRequest>().Without(x => x.Id).Create();
        var itemAdded02 = _fixture.Build<WorkExperienceRequest>().Without(x => x.Id).Create();
        var itemUpdated = _fixture.Build<WorkExperienceRequest>().Without(x => x.Id).Create();

        var initialSummaryEntity = await SetupEntityAsync(() => _fixture.BuildSummaryEntityWithRelations());
        var itemToReplace = initialSummaryEntity.WorkExperiences.ElementAt(3);
        var itemToRemove = initialSummaryEntity.WorkExperiences.ElementAt(1);
        itemUpdated.Id = itemToReplace.Id;
        var indexItemToReplace =
            initialSummaryEntity.WorkExperiences.ToList().FindIndex(item => item.Id == itemToReplace.Id);
        var indexItemToRemove =
            initialSummaryEntity.WorkExperiences.ToList().FindIndex(item => item.Id == itemToRemove.Id);

        var patchDocument = new JsonPatchDocument<SummaryComplexRequest>();
        patchDocument.Add(x => x.WorkExperiences, itemAdded01);
        patchDocument.Add(x => x.WorkExperiences, itemAdded02);
        patchDocument.Replace(p => p.WorkExperiences[indexItemToReplace], itemUpdated);
        patchDocument.Remove(p => p.WorkExperiences, indexItemToRemove);

        // // When
        var response =
            await _httpClient.PatchAsNewtonsoftJsonAsync(
                $"{RequestUriController}/{initialSummaryEntity.Id}/WorkExperience", patchDocument);

        // Then
        response.IsSuccessStatusCode.ShouldBeTrue();
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);

        await using var scope = _testFixture.GetProvider().CreateAsyncScope();
        var querySkillRepository = scope.ServiceProvider.GetRequiredService<ISummaryQueryRepository>();
        _ = querySkillRepository.IncludeWorkExperience();
        var summaryEntity = await querySkillRepository.GetByIdWithIncludesAsync(initialSummaryEntity.Id);
        summaryEntity.ShouldNotBeNull();
        summaryEntity.WorkExperiences.Count.ShouldBe(initialSummaryEntity.WorkExperiences.Count + 1);
        summaryEntity.WorkExperiences.Should().ContainEquivalentOf(itemAdded01,
            options => options.ExcludingMissingMembers().Excluding(x => x.Id));
        summaryEntity.WorkExperiences.Should().ContainEquivalentOf(itemAdded02,
            options => options.ExcludingMissingMembers().Excluding(x => x.Id));
        summaryEntity.WorkExperiences.Should().ContainEquivalentOf(itemUpdated,
            options => options.ExcludingMissingMembers());
        summaryEntity.WorkExperiences.Contains(itemToRemove).ShouldBeFalse();
    }

    private async Task<SummaryEntity> SetupEntityAsync(Func<SummaryEntity> entityBuilder)
    {
        var summaryEntity = entityBuilder();
        await _testFixture.ResetDatabaseAsync();
        return await CreateAndPersistEntityAsync(summaryEntity);
    }

    private async Task<ProfileEntity> SetupProfileEntityAsync()
    {
        await _testFixture.ResetDatabaseAsync();
        await using var scope = _testFixture.GetProvider().CreateAsyncScope();
        var commandRepository = scope.ServiceProvider.GetRequiredService<ICommandRepository<ProfileEntity>>();

        var entity = _fixture.GetProfileEntityComposer().Create();
        await commandRepository.CreateAsync(entity);

        return entity;
    }

    private async Task<SummaryEntity> CreateAndPersistEntityAsync(SummaryEntity entity)
    {
        await using var scope = _testFixture.GetProvider().CreateAsyncScope();
        var commandRepository = scope.ServiceProvider.GetRequiredService<ICommandRepository<SummaryEntity>>();

        await commandRepository.CreateAsync(entity);

        return entity;
    }
}