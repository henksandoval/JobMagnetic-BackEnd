using System.Net;
using AwesomeAssertions;
using JobMagnet.Domain.Aggregates.Profiles;
using JobMagnet.Domain.Ports.Repositories;
using JobMagnet.Infrastructure.Persistence.Context;
using JobMagnet.Integration.Tests.Fixtures;
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
    private readonly ITestOutputHelper _testOutputHelper = testFixture.SetTestOutputHelper(testOutputHelper);

    [Fact(DisplayName = "Return 200 and Pong when GET ping request is called", Skip = "Skipped")]
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

    [Fact(DisplayName = "Delete and return 204 when DELETE request is received", Skip = "Skipped")]
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

    [Fact(DisplayName = "Create and return 200 when Post request is received", Skip = "Skipped")]
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

    [Fact(DisplayName = "Return 200 when Post seedProfile request is received", Skip = "Skipped")]
    public async Task SeedData_WhenPostSeedProfileRequestIsReceivedIsAsync()
    {
        // --- When ---
        var response = await _httpClient.PostAsync($"{RequestUriController}/seedProfile", null, CancellationToken.None);

        // --- Then ---
        response.IsSuccessStatusCode.Should().BeTrue();
        response.StatusCode.Should().Be(HttpStatusCode.Accepted);

        var profileId = new ProfileId();
        await using var scope = testFixture.GetProvider().CreateAsyncScope();
        var profileQueryRepository = scope.ServiceProvider.GetRequiredService<IProfileQueryRepository>();
        var profile = await profileQueryRepository
            .WithResume()
            .WithTalents()
            .WithTestimonials()
            .WithSkills()
            .WithProject()
            .WhereCondition(x => x.Id == profileId)
            .BuildFirstOrDefaultAsync(CancellationToken.None);

        profile.Should().NotBeNull();
        // profile.SkillSet.Skills.Count.Should().Be(SkillInfoCollection.Data.Count);
        // profile.Talents.Count.Should().Be(new TalentsSeeder(profileId).GetTalents().Count);
        // profile.Testimonials.Count.Should().Be(new TestimonialSeeder(profileId).GetTestimonials().Count);
        // profile.Projects.Count.Should().Be(new ProjectSeeder(profileId).GetProjects().Count);
    }
}