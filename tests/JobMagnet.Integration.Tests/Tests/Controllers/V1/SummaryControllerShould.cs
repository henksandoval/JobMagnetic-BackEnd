using System.Net;
using AutoFixture;
using AwesomeAssertions;
using JobMagnet.Application.Contracts.Commands.Summary;
using JobMagnet.Application.Contracts.Responses.Base;
using JobMagnet.Application.Contracts.Responses.Summary;
using JobMagnet.Domain.Aggregates.Profiles;
using JobMagnet.Domain.Aggregates.Profiles.Entities;
using JobMagnet.Domain.Ports.Repositories;
using JobMagnet.Domain.Ports.Repositories.Base;
using JobMagnet.Integration.Tests.Fixtures;
using JobMagnet.Shared.Tests.Fixtures;
using JobMagnet.Shared.Tests.Fixtures.Builders;
using JobMagnet.Shared.Tests.Utils;
using Microsoft.Extensions.DependencyInjection;

using Xunit.Abstractions;

namespace JobMagnet.Integration.Tests.Tests.Controllers.V1;

public class SummaryControllerShould : IClassFixture<JobMagnetTestSetupFixture>
{
    private const string RequestUriController = "api/v1/CareerHistory";
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

    [Fact(DisplayName = "Create a new record and return 201 when the POST request is valid")]
    public async Task ReturnCreatedAndPersistData_WhenRequestIsValidAsync()
    {
        // --- Given ---
        await _testFixture.ResetDatabaseAsync();
        var profileEntity = await SetupProfileEntityAsync();
        var educationCollection = _fixture.CreateMany<EducationBase>(3).ToList();
        var workExperienceCollection = _fixture.CreateMany<WorkExperienceBase>(3).ToList();
        var summaryBase = _fixture.Build<SummaryBase>()
            .With(x => x.ProfileId, profileEntity.Id.Value)
            .With(x => x.Education, educationCollection)
            .With(x => x.WorkExperiences, workExperienceCollection)
            .Create();

        var createRequest = _fixture.Build<SummaryCommand>()
            .With(x => x.SummaryData, summaryBase)
            .Create();
        var httpContent = TestUtilities.SerializeRequestContent(createRequest);

        // --- When ---
        var response = await _httpClient.PostAsync(RequestUriController, httpContent);

        // --- Then ---
        response.IsSuccessStatusCode.Should().BeTrue();
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var responseData = await TestUtilities.DeserializeResponseAsync<SummaryResponse>(response);
        responseData.Should().NotBeNull();

        var locationHeader = response.Headers.Location!.ToString();
        locationHeader.Should().NotBeNull();
        locationHeader.Should().Contain($"{RequestUriController}/{responseData.Id}");

        await using var scope = _testFixture.GetProvider().CreateAsyncScope();
        var queryRepository = scope.ServiceProvider.GetRequiredService<ISummaryQueryRepository>();
        var entityCreated = await queryRepository.GetByIdWithIncludesAsync(new CareerHistoryId(responseData.Id));

        entityCreated.Should().NotBeNull();
        entityCreated.Qualifications.Should().NotBeNull();
        entityCreated.WorkExperiences.Should().NotBeNull();
        entityCreated.Qualifications.Should().BeSameAs(entityCreated.Qualifications);
        entityCreated.WorkExperiences.Should().BeSameAs(entityCreated.WorkExperiences);
    }

    [Fact(DisplayName = "Return the record and return 200 when GET request with valid ID is provided")]
    public async Task ReturnRecord_WhenValidIdIsProvidedAsync()
    {
        // --- Given ---
        var entity = await SetupEntityAsync(() => _fixture.Create<CareerHistory>());

        // --- When ---
        var response = await _httpClient.GetAsync($"{RequestUriController}/{entity.Id}");

        // --- Then ---
        response.IsSuccessStatusCode.Should().BeTrue();
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var responseData = await TestUtilities.DeserializeResponseAsync<SummaryResponse>(response);
        responseData.Should().NotBeNull();
        responseData.Should().BeEquivalentTo(entity, options => options.ExcludingMissingMembers());
    }

    [Fact(DisplayName = "Return 404 when GET request with invalid ID is provided")]
    public async Task ReturnNotFound_WhenInvalidIdIsProvidedAsync()
    {
        // --- Given ---
        _ = await SetupEntityAsync(() => _fixture.Create<CareerHistory>());

        // --- When ---
        var response = await _httpClient.GetAsync($"{RequestUriController}/{InvalidId}");

        // --- Then ---
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact(DisplayName = "Delete and return 204 when DELETE request is received")]
    public async Task DeleteRecord_WhenDeleteRequestIsReceivedAsync()
    {
        // --- Given ---
        var entity = await SetupEntityAsync(() => _fixture.Create<CareerHistory>());

        // --- When ---
        var response = await _httpClient.DeleteAsync($"{RequestUriController}/{entity.Id}");

        // --- Then ---
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        await using var scope = _testFixture.GetProvider().CreateAsyncScope();
        var querySummaryRepository = scope.ServiceProvider.GetRequiredService<ISummaryQueryRepository>();
        var summaryEntity = await querySummaryRepository.GetByIdAsync(entity.Id, CancellationToken.None);
        summaryEntity.Should().BeNull();
    }

    [Fact(DisplayName = "Return 404 when a DELETE request with invalid ID is provided")]
    public async Task ReturnNotFound_WhenDeleteRequestWithInvalidIdIsProvidedAsync()
    {
        // --- Given ---
        _ = await SetupEntityAsync(() => _fixture.Create<CareerHistory>());

        // --- When ---
        var response = await _httpClient.DeleteAsync($"{RequestUriController}/{InvalidId}");

        // --- Then ---
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    private async Task<CareerHistory> SetupEntityAsync(Func<CareerHistory> entityBuilder)
    {
        var summaryEntity = entityBuilder();
        await _testFixture.ResetDatabaseAsync();
        return await CreateAndPersistEntityAsync(summaryEntity);
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

    private async Task<CareerHistory> CreateAndPersistEntityAsync(CareerHistory careerHistory)
    {
        await using var scope = _testFixture.GetProvider().CreateAsyncScope();
        var commandRepository = scope.ServiceProvider.GetRequiredService<ICommandRepository<Profile>>();

        var entity = new ProfileEntityBuilder(_fixture)
            .WithSummary(careerHistory)
            .Build();
        await commandRepository.CreateAsync(entity);
        await commandRepository.SaveChangesAsync();

        return entity.ProfessionalSummary!;
    }
}