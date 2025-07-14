using System.Net;
using AwesomeAssertions;
using JobMagnet.Domain.Aggregates.Profiles.ValueObjects;
using JobMagnet.Domain.Ports.Repositories;
using JobMagnet.Infrastructure.Persistence.Context;
using JobMagnet.Infrastructure.Persistence.Seeders;
using JobMagnet.Infrastructure.Persistence.Seeders.Collections;
using JobMagnet.Integration.Tests.Fixtures;
using JobMagnet.Shared.Abstractions;
using JobMagnet.Shared.Data;
using JobMagnet.Shared.Tests.Abstractions;
using JobMagnet.Shared.Tests.Utils;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace JobMagnet.Integration.Tests.Tests.Controllers.V0;

public class AdminControllerShould(
    JobMagnetTestSetupFixture testFixture,
    ITestOutputHelper testOutputHelper)
    : IClassFixture<JobMagnetTestSetupFixture>
{
    private const string RequestUriController = "api/v0.1/admin";
    private readonly HttpClient _httpClient = testFixture.GetClient();
    private readonly IGuidGenerator _guidGenerator = new SequentialGuidGenerator();
    private readonly ITestOutputHelper _testOutputHelper = testFixture.SetTestOutputHelper(testOutputHelper);

    [Fact(DisplayName = "Return 200 and Pong when GET ping request is called")]
    public async Task ReturnPong_WhenGetPingRequestAsync()
    {
        // --- When ---
        var response = await _httpClient.GetAsync($"{RequestUriController}/ping");

        // --- Then ---
        response.IsSuccessStatusCode.Should().BeTrue();
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var responseData = await response.Content.ReadAsStringAsync();
        responseData.Should().NotBeNullOrEmpty();
        responseData.Should().Be("Pong");
    }

    [Fact(DisplayName = "Delete and return 204 when DELETE request is received")]
    public async Task DestroyDatabase_WhenDeleteRequestIsReceivedIsAsync()
    {
        // --- Given ---
        await using var scope = testFixture.GetProvider().CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<JobMagnetDbContext>();
        await dbContext.Database.EnsureCreatedAsync();
        var canConnect = await dbContext.Database.CanConnectAsync();
        _testOutputHelper.WriteLine("Database connection status: {0}", canConnect);

        // --- When ---
        var response = await _httpClient.DeleteAsync($"{RequestUriController}");

        // --- Then ---
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        canConnect = await dbContext.Database.CanConnectAsync();
        canConnect.Should().BeFalse();
    }

    [Fact(DisplayName = "Create and return 200 when Post request is received")]
    public async Task CreateDatabase_WhenPostRequestIsReceivedIsAsync()
    {
        // --- When ---
        var response = await _httpClient.PostAsync($"{RequestUriController}", null);

        // --- Then ---
        response.IsSuccessStatusCode.Should().BeTrue();
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        await using var scope = testFixture.GetProvider().CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<JobMagnetDbContext>();
        var canConnect = await dbContext.Database.CanConnectAsync();
        canConnect.Should().BeTrue();
    }

    [Fact(DisplayName = "Return 200 when Post seedProfile request is received")]
    public async Task SeedData_WhenPostSeedProfileRequestIsReceivedIsAsync()
    {
        // --- When ---
        var response = await _httpClient.PostAsync($"{RequestUriController}/seedProfile", null, CancellationToken.None);

        // --- Then ---
        response.IsSuccessStatusCode.Should().BeTrue();
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var profileId = await TestUtilities.DeserializeResponseAsync<Guid>(response);
        await using var scope = testFixture.GetProvider().CreateAsyncScope();
        var profileQueryRepository = scope.ServiceProvider.GetRequiredService<IProfileQueryRepository>();
        var profile = await profileQueryRepository
            .WithProfileHeader()
            .WithTalents()
            .WithTestimonials()
            .WithProfileHeader()
            .WithProject()
            .WithSkills()
            .WithCareerHistory()
            .WhereCondition(x => x.Id == new ProfileId(profileId))
            .BuildFirstOrDefaultAsync(CancellationToken.None);

        profile.Should().NotBeNull();
        profile.GetSkills().Should().HaveSameCount(SkillInfoCollection.Data);
        profile.CareerHistory!.AcademicDegree.Should().HaveCount(CareerHistorySeeder.AcademicDegreeCount);
        profile.CareerHistory!.WorkExperiences.Should().HaveCount(CareerHistorySeeder.WorkExperienceCount);
        profile.TalentShowcase.Should().HaveSameCount(new TalentsSeeder(_guidGenerator, new ProfileId()).GetTalents());
        profile.Testimonials.Count.Should().Be(Seeder.TestimonialsCount);
        profile.Portfolio.Should().HaveSameCount(ProjectRawData.Data);
    }
}