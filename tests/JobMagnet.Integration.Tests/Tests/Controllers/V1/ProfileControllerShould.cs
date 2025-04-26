using System.Net;
using System.Net.Http.Json;
using AutoFixture;
using FluentAssertions;
using JobMagnet.Infrastructure.Entities;
using JobMagnet.Infrastructure.Repositories.Base.Interfaces;
using JobMagnet.Infrastructure.Repositories.Interfaces;
using JobMagnet.Integration.Tests.Fixtures;
using JobMagnet.Models.Profile;
using JobMagnet.Models.Queries.Profile;
using JobMagnet.Shared.Tests.Fixtures;
using JobMagnet.Shared.Tests.Fixtures.Builders;
using JobMagnet.Shared.Tests.Utils;
using JobMagnet.ViewModels.Profile;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit.Abstractions;

namespace JobMagnet.Integration.Tests.Tests.Controllers.V1;

public class ProfileControllerShould : IClassFixture<JobMagnetTestSetupFixture>
{
    private const string RequestUriController = "api/v1/profile";
    private const string InvalidId = "100";
    private const int ContactInfoCount = 3;
    private const int TalentsCount = 8;
    private const int PortfolioCount = 3;
    private const int SummariesCount = 3;
    private const int TestimonialsCount = 12;
    private readonly IFixture _fixture = FixtureBuilder.Build();
    private readonly HttpClient _httpClient;
    private readonly JobMagnetTestSetupFixture _testFixture;

    public ProfileControllerShould(JobMagnetTestSetupFixture testFixture, ITestOutputHelper testOutputHelper)
    {
        _testFixture = testFixture;
        _httpClient = _testFixture.GetClient();
        _testFixture.SetTestOutputHelper(testOutputHelper);
    }

    [Fact(DisplayName = "Return the record and return 200 when GET request with valid Name is provided")]
    public async Task ReturnRecord_WhenValidNameProvidedAsync()
    {
        // Given
        var entity = await SetupEntityAsync();
        var queryParameters = new Dictionary<string, string>
        {
            { nameof(ProfileQueryParameters.Name), entity.FirstName }
        };

        var requestUrl = QueryHelpers.AddQueryString(RequestUriController, queryParameters!);

        // When
        var response = await _httpClient.GetAsync(requestUrl);

        // Then
        response.IsSuccessStatusCode.ShouldBeTrue();
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var responseData = await TestUtilities.DeserializeResponseAsync<ProfileViewModel>(response);
        responseData.ShouldNotBeNull();
        responseData.ShouldBeAssignableTo<ProfileViewModel>();

        responseData.PersonalData.ShouldNotBeNull();
        responseData.PersonalData.SocialNetworks.Length.ShouldBe(ContactInfoCount);
        responseData.About.ShouldNotBeNull();
        responseData.Testimonials!.Length.ShouldBe(TestimonialsCount);
        responseData.SkillSet.ShouldNotBeNull();
        responseData.PortfolioGallery!.Length.ShouldBe(PortfolioCount);
    }

    [Fact(DisplayName = "Create a new record and return 201 when the POST request is valid")]
    public async Task ReturnCreatedAndPersistData_WhenRequestIsValidAsync()
    {
        // Given
        await _testFixture.ResetDatabaseAsync();
        var createRequest = _fixture.Build<ProfileCreateRequest>().Create();
        var httpContent = TestUtilities.SerializeRequestContent(createRequest);

        // When
        var response = await _httpClient.PostAsync(RequestUriController, httpContent);

        // Then
        response.IsSuccessStatusCode.ShouldBeTrue();
        response.StatusCode.ShouldBe(HttpStatusCode.Created);

        var responseData = await TestUtilities.DeserializeResponseAsync<ProfileModel>(response);
        responseData.ShouldNotBeNull();

        var locationHeader = response.Headers.Location!.ToString();
        locationHeader.ShouldNotBeNull();
        locationHeader.ShouldContain($"{RequestUriController}/{responseData.Id}");

        await using var scope = _testFixture.GetProvider().CreateAsyncScope();
        var queryRepository = scope.ServiceProvider.GetRequiredService<IProfileQueryRepository>();
        var entityCreated = await queryRepository.GetByIdAsync(responseData.Id);

        entityCreated.ShouldNotBeNull();
        entityCreated.Should().BeEquivalentTo(createRequest, options => options.ExcludingMissingMembers());
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

        var responseData = await TestUtilities.DeserializeResponseAsync<ProfileModel>(response);
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


    [Fact(DisplayName = "Return 204 when a valid PUT request is provided")]
    public async Task ReturnNotContent_WhenReceivedValidPutRequestAsync()
    {
        // Given
        var entity = await SetupEntityAsync();
        var updateRequest = _fixture.Build<ProfileUpdateRequest>()
            .With(x => x.Id, entity.Id)
            .Create();

        // When
        var response = await _httpClient.PutAsJsonAsync($"{RequestUriController}/{entity.Id}", updateRequest);

        // Then
        response.IsSuccessStatusCode.ShouldBeTrue();
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);

        await using var scope = _testFixture.GetProvider().CreateAsyncScope();
        var queryRepository = scope.ServiceProvider.GetRequiredService<IProfileQueryRepository>();
        var dbEntity = await queryRepository.GetByIdAsync(entity.Id);
        dbEntity.ShouldNotBeNull();
        dbEntity.Should().BeEquivalentTo(updateRequest, options => options.ExcludingMissingMembers());
    }

    [Fact(DisplayName = "Return 400 when a PUT request with invalid ID is provided")]
    public async Task ReturnBadRequest_WhenPutRequestWithInvalidIdIsProvidedAsync()
    {
        // Given
        await _testFixture.ResetDatabaseAsync();
        var updatedEntity = _fixture.Build<ProfileUpdateRequest>().Create();
        var differentId = updatedEntity.Id + InvalidId;

        // When
        var response = await _httpClient.PutAsJsonAsync($"{RequestUriController}/{differentId}", updatedEntity);

        // Then
        response.IsSuccessStatusCode.ShouldBeFalse();
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }

    [Fact(DisplayName = "Return 404 when a PUT request with invalid ID is provided")]
    public async Task ReturnNotFound_WhenPutRequestWithInvalidIdIsProvidedAsync()
    {
        // Given
        await _testFixture.ResetDatabaseAsync();
        var updatedEntity = _fixture.Build<ProfileUpdateRequest>().Create();

        // When
        var response = await _httpClient.PutAsJsonAsync($"{RequestUriController}/{updatedEntity.Id}", updatedEntity);

        // Then
        response.IsSuccessStatusCode.ShouldBeFalse();
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    private async Task<ProfileEntity> SetupEntityAsync()
    {
        await _testFixture.ResetDatabaseAsync();
        return await CreateAndPersistEntityAsync();
    }

    private async Task<ProfileEntity> CreateAndPersistEntityAsync()
    {
        await using var scope = _testFixture.GetProvider().CreateAsyncScope();
        var commandRepository = scope.ServiceProvider.GetRequiredService<ICommandRepository<ProfileEntity>>();

        var entity = new ProfileEntityBuilder(_fixture)
            .WithResume()
            .WithContactInfo(ContactInfoCount)
            .WithTalents(TalentsCount)
            .WithPortfolio(PortfolioCount)
            .WithSummaries(SummariesCount)
            .WithServices()
            .WithSkills()
            .WithTestimonials(TestimonialsCount)
            .Build();

        await commandRepository.CreateAsync(entity);

        return entity;
    }
}