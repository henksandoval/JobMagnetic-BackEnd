using System.Net;
using AutoFixture;
using FluentAssertions;
using JobMagnet.Infrastructure.Entities;
using JobMagnet.Infrastructure.Repositories.Base.Interfaces;
using JobMagnet.Integration.Tests.Fixtures;
using JobMagnet.Shared.Tests.Utils;
using JobMagnet.Models.Queries.Profile;
using JobMagnet.Shared.Tests.Fixtures;
using JobMagnet.Shared.Tests.Fixtures.Builders;
using JobMagnet.ViewModels.Profile;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit.Abstractions;

namespace JobMagnet.Integration.Tests.Tests.Controllers.V1;

public class ProfileControllerTests : IClassFixture<JobMagnetTestSetupFixture>
{
    private const string RequestUriController = "api/v1/Profile";
    private readonly IFixture _fixture = FixtureBuilder.Build();
    private readonly HttpClient _httpClient;
    private readonly JobMagnetTestSetupFixture _testFixture;

    public ProfileControllerTests(JobMagnetTestSetupFixture testFixture, ITestOutputHelper testOutputHelper)
    {
        _testFixture = testFixture;
        _httpClient = _testFixture.GetClient();
        _testFixture.SetTestOutputHelper(testOutputHelper);
    }

    [Fact(DisplayName = "Should return the record and return 200 when GET request with valid Name is provided")]
    public async Task ShouldReturnRecord_WhenValidNameProvidedAsync()
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
        responseData.About.ShouldNotBeNull();
        responseData.Testimonials.ShouldNotBeNull();
        responseData.SkillSet.ShouldNotBeNull();
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
            .WithTalents()
            .WithPortfolio()
            .WithSummaries()
            .WithServices()
            .WithSkills()
            .WithTestimonials()
            .Build();

        await commandRepository.CreateAsync(entity);

        return entity;
    }
}