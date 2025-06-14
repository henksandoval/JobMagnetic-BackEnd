using System.Net;
using AutoFixture;
using FluentAssertions;
using JobMagnet.Application.Contracts.Commands.Summary;
using JobMagnet.Application.Contracts.Responses.Base;
using JobMagnet.Application.Contracts.Responses.Summary;
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

public class SummaryControllerShould : IClassFixture<JobMagnetTestSetupFixture>
{
    private const string RequestUriController = "api/v1/Summary";
    private const string InvalidId = "100";
    private readonly IFixture _fixture = FixtureBuilder.Build();
    private readonly HttpClient _httpClient;
    private readonly JobMagnetTestSetupFixture _testFixture;

    public SummaryControllerShould(JobMagnetTestSetupFixture testFixture, ITestOutputHelper testOutputHelper)
    {
        _testFixture = testFixture;
        _httpClient = _testFixture.GetClient();
        _testFixture.SetTestOutputHelper(testOutputHelper);
    }

    [Fact(DisplayName = "Create a new record and return 201 when the POST request is valid", Skip = "TODO: Fix this test")]
    public async Task ReturnCreatedAndPersistData_WhenRequestIsValidAsync()
    {
        // Given
        await _testFixture.ResetDatabaseAsync();
        var profileEntity = await SetupProfileEntityAsync();
        var educationCollection = _fixture.Build<EducationBase>().With(x => x.Id, 0).CreateMany(3).ToList();
        var workExperienceCollection = _fixture.Build<WorkExperienceBase>().With(x => x.Id, 0).CreateMany(3).ToList();
        var summaryBase = _fixture.Build<SummaryBase>()
            .With(x => x.ProfileId, profileEntity.Id)
            .With(x => x.Education, educationCollection)
            .With(x => x.WorkExperiences, workExperienceCollection)
            .Create();

        var createRequest = _fixture.Build<SummaryCommand>()
            .With(x => x.SummaryData, summaryBase)
            .Create();
        var httpContent = TestUtilities.SerializeRequestContent(createRequest);

        // When
        var response = await _httpClient.PostAsync(RequestUriController, httpContent);

        // Then
        response.IsSuccessStatusCode.ShouldBeTrue();
        response.StatusCode.ShouldBe(HttpStatusCode.Created);

        var responseData = await TestUtilities.DeserializeResponseAsync<SummaryResponse>(response);
        responseData.ShouldNotBeNull();

        var locationHeader = response.Headers.Location!.ToString();
        locationHeader.ShouldNotBeNull();
        locationHeader.ShouldContain($"{RequestUriController}/{responseData.Id}");

        await using var scope = _testFixture.GetProvider().CreateAsyncScope();
        var queryRepository = scope.ServiceProvider.GetRequiredService<ISummaryQueryRepository>();
        var entityCreated = await queryRepository.GetByIdWithIncludesAsync(responseData.Id);

        entityCreated.ShouldNotBeNull();
    }

    [Fact(DisplayName = "Return the record and return 200 when GET request with valid ID is provided")]
    public async Task ReturnRecord_WhenValidIdIsProvidedAsync()
    {
        // Given
        var entity = await SetupEntityAsync(() => _fixture.Create<SummaryEntity>());

        // When
        var response = await _httpClient.GetAsync($"{RequestUriController}/{entity.Id}");

        // Then
        response.IsSuccessStatusCode.ShouldBeTrue();
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var responseData = await TestUtilities.DeserializeResponseAsync<SummaryResponse>(response);
        responseData.ShouldNotBeNull();
        responseData.Should().BeEquivalentTo(entity, options => options.ExcludingMissingMembers());
    }

    [Fact(DisplayName = "Return 404 when GET request with invalid ID is provided")]
    public async Task ReturnNotFound_WhenInvalidIdIsProvidedAsync()
    {
        // Given
        _ = await SetupEntityAsync(() => _fixture.Create<SummaryEntity>());

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
        var entity = await SetupEntityAsync(() => _fixture.Create<SummaryEntity>());

        // When
        var response = await _httpClient.DeleteAsync($"{RequestUriController}/{entity.Id}");

        // Then
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);

        await using var scope = _testFixture.GetProvider().CreateAsyncScope();
        var querySummaryRepository = scope.ServiceProvider.GetRequiredService<ISummaryQueryRepository>();
        var summaryEntity = await querySummaryRepository.GetByIdAsync(entity.Id);
        summaryEntity.ShouldBeNull();
    }

    [Fact(DisplayName = "Return 404 when a DELETE request with invalid ID is provided")]
    public async Task ReturnNotFound_WhenDeleteRequestWithInvalidIdIsProvidedAsync()
    {
        // Given
        _ = await SetupEntityAsync(() => _fixture.Create<SummaryEntity>());

        // When
        var response = await _httpClient.DeleteAsync($"{RequestUriController}/{InvalidId}");

        // Then
        response.IsSuccessStatusCode.ShouldBeFalse();
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    [Fact(DisplayName = "Handle multiple Education Add operations in a PATCH request")]
    public async Task HandleAddMultipleEducationOperationsInPatchEducationRequestAsync()
    {
        // Given
        var summary = await SetupEntityAsync(() => _fixture.Create<SummaryEntity>());
        var itemAdded01 = _fixture.Build<EducationBase>().Without(x => x.Id).Create();
        var itemAdded02 = _fixture.Build<EducationBase>().Without(x => x.Id).Create();
        var patchDocument = new JsonPatchDocument<SummaryCommand>();
        patchDocument.Add(x => x.SummaryData.Education, itemAdded01);
        patchDocument.Add(x => x.SummaryData.Education, itemAdded02);

        // When
        var response =
            await _httpClient.PatchAsNewtonsoftJsonAsync($"{RequestUriController}/{summary.Id}",
                patchDocument);

        // Then
        response.IsSuccessStatusCode.ShouldBeTrue();
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);

        await using var scope = _testFixture.GetProvider().CreateAsyncScope();
        var querySkillRepository = scope.ServiceProvider.GetRequiredService<ISummaryQueryRepository>();
        _ = querySkillRepository.IncludeEducation();
        var summaryEntity = await querySkillRepository.GetByIdWithIncludesAsync(summary.Id);
        summaryEntity.ShouldNotBeNull();
        summaryEntity.Education.Count.ShouldBe(summary.Education.Count + 2);
        summaryEntity.Education.Should().ContainEquivalentOf(itemAdded01,
            options => options.ExcludingMissingMembers().Excluding(x => x.Id));
    }

    [Fact(DisplayName = "Handle multiple Education operations in a PATCH request")]
    public async Task HandleMultipleEducationOperationsInPatchEducationRequestAsync()
    {
        // Given
        var entity = new SummaryEntityBuilder(_fixture).WithEducation().WithWorkExperiences().Build();
        var initialSummaryEntity = await SetupEntityAsync(() => entity);
        var itemToReplace = initialSummaryEntity.Education.ElementAt(3);
        var itemToRemove = initialSummaryEntity.Education.ElementAt(1);

        var itemAdded01 = _fixture.Build<EducationBase>().Without(x => x.Id).Create();
        var itemAdded02 = _fixture.Build<EducationBase>().Without(x => x.Id).Create();
        var itemUpdated = _fixture.Build<EducationBase>().With(x => x.Id, itemToReplace.Id).Create();

        var indexItemToReplace = initialSummaryEntity.Education.ToList().FindIndex(item => item.Id == itemToReplace.Id);
        var indexItemToRemove = initialSummaryEntity.Education.ToList().FindIndex(item => item.Id == itemToRemove.Id);

        var patchDocument = new JsonPatchDocument<SummaryCommand>();
        patchDocument.Add(x => x.SummaryData.Education, itemAdded01);
        patchDocument.Add(x => x.SummaryData.Education, itemAdded02);
        patchDocument.Replace(p => p.SummaryData.Education[indexItemToReplace], itemUpdated);
        patchDocument.Remove(p => p.SummaryData.Education, indexItemToRemove);

        // When
        var response =
            await _httpClient.PatchAsNewtonsoftJsonAsync($"{RequestUriController}/{initialSummaryEntity.Id}",
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

    [Fact(DisplayName = "Handle multiple Work Experience Add operations in a PATCH request", Skip = "TODO: Fix this test")]
    public async Task HandleAddMultipleWorkExperienceOperationsInPatchWorkExperienceRequestAsync()
    {
        // Given
        var summary = await SetupEntityAsync(() => _fixture.Create<SummaryEntity>());
        var workExperiences = summary.WorkExperiences;
        var itemAdded01 = _fixture.Build<WorkExperienceBase>().Without(x => x.Id).Create();
        var itemAdded02 = _fixture.Build<WorkExperienceBase>().Without(x => x.Id).Create();
        var patchDocument = new JsonPatchDocument<SummaryCommand>();
        patchDocument.Add(x => x.SummaryData.WorkExperiences, itemAdded01);
        patchDocument.Add(x => x.SummaryData.WorkExperiences, itemAdded02);

        // When
        var response =
            await _httpClient.PatchAsNewtonsoftJsonAsync($"{RequestUriController}/{summary.Id}",
                patchDocument);

        // Then
        response.IsSuccessStatusCode.ShouldBeTrue();
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);

        await using var scope = _testFixture.GetProvider().CreateAsyncScope();
        var querySkillRepository = scope.ServiceProvider.GetRequiredService<ISummaryQueryRepository>();
        _ = querySkillRepository.IncludeWorkExperience();
        var summaryEntity = await querySkillRepository.GetByIdWithIncludesAsync(summary.Id);
        summaryEntity.ShouldNotBeNull();
        summaryEntity.WorkExperiences.Count.ShouldBe(workExperiences.Count + 2);
        summaryEntity.WorkExperiences.Should().ContainEquivalentOf(itemAdded01,
            options => options.ExcludingMissingMembers().Excluding(x => x.Id));
    }

    [Fact(DisplayName = "Handle multiple Work Experience in a PATCH request", Skip = "TODO: Fix this test")]
    public async Task HandleMultipleWorkExperienceOperationsInPatchRequestAsync()
    {
        // Given
        var entity = new SummaryEntityBuilder(_fixture).WithEducation().WithWorkExperiences().Build();
        var initialSummaryEntity = await SetupEntityAsync(() => entity);
        var itemToReplace = initialSummaryEntity.WorkExperiences.ElementAt(3);
        var itemToRemove = initialSummaryEntity.WorkExperiences.ElementAt(1);

        var itemAdded01 = _fixture.Build<WorkExperienceBase>().Without(x => x.Id).Create();
        var itemAdded02 = _fixture.Build<WorkExperienceBase>().Without(x => x.Id).Create();
        var itemUpdated = _fixture.Build<WorkExperienceBase>().With(x => x.Id, itemToReplace.Id).Create();

        var indexItemToReplace =
            initialSummaryEntity.WorkExperiences.ToList().FindIndex(item => item.Id == itemToReplace.Id);
        var indexItemToRemove =
            initialSummaryEntity.WorkExperiences.ToList().FindIndex(item => item.Id == itemToRemove.Id);

        var patchDocument = new JsonPatchDocument<SummaryCommand>();
        patchDocument.Add(x => x.SummaryData.WorkExperiences, itemAdded01);
        patchDocument.Add(x => x.SummaryData.WorkExperiences, itemAdded02);
        patchDocument.Replace(p => p.SummaryData.WorkExperiences[indexItemToReplace], itemUpdated);
        patchDocument.Remove(p => p.SummaryData.WorkExperiences, indexItemToRemove);

        // // When
        var response =
            await _httpClient.PatchAsNewtonsoftJsonAsync(
                $"{RequestUriController}/{initialSummaryEntity.Id}", patchDocument);

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

        var entity = new ProfileEntityBuilder(_fixture).Build();
        await commandRepository.CreateAsync(entity);
        await commandRepository.SaveChangesAsync();

        return entity;
    }

    private async Task<SummaryEntity> CreateAndPersistEntityAsync(SummaryEntity entity)
    {
        await using var scope = _testFixture.GetProvider().CreateAsyncScope();
        var commandRepository = scope.ServiceProvider.GetRequiredService<ICommandRepository<SummaryEntity>>();

        await commandRepository.CreateAsync(entity);
        await commandRepository.SaveChangesAsync();

        return entity;
    }
}