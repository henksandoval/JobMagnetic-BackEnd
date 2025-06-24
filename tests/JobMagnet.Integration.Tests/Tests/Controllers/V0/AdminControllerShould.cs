using System.Net;
using JobMagnet.Domain.Ports.Repositories;
using JobMagnet.Infrastructure.Persistence.Context;
using JobMagnet.Infrastructure.Persistence.Seeders.Collections;
using JobMagnet.Integration.Tests.Fixtures;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
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

    [Fact(DisplayName = "Return 200 and Pong when GET ping request is called")]
    public async Task ReturnPong_WhenGetPingRequestAsync()
    {
        // --- When ---
        var response = await _httpClient.GetAsync($"{RequestUriController}/ping");

        // --- Then ---
        response.IsSuccessStatusCode.ShouldBeTrue();
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var responseData = await response.Content.ReadAsStringAsync();
        responseData.ShouldNotBeNullOrEmpty();
        responseData.ShouldBe("Pong");
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
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);

        canConnect = await dbContext.Database.CanConnectAsync();
        canConnect.ShouldBeFalse();
    }

    [Fact(DisplayName = "Create and return 200 when Post request is received")]
    public async Task CreateDatabase_WhenPostRequestIsReceivedIsAsync()
    {
        // --- When ---
        var response = await _httpClient.PostAsync($"{RequestUriController}", null);

        // --- Then ---
        response.IsSuccessStatusCode.ShouldBeTrue();
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        await using var scope = testFixture.GetProvider().CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<JobMagnetDbContext>();
        var canConnect = await dbContext.Database.CanConnectAsync();
        canConnect.ShouldBeTrue();
    }

    [Fact(DisplayName = "Return 200 when Post seedProfile request is received")]
    public async Task SeedData_WhenPostSeedProfileRequestIsReceivedIsAsync()
    {
        // --- When ---
        var response = await _httpClient.PostAsync($"{RequestUriController}/seedProfile", null, CancellationToken.None);

        // --- Then ---
        response.IsSuccessStatusCode.ShouldBeTrue();
        response.StatusCode.ShouldBe(HttpStatusCode.Accepted);

        await using var scope = testFixture.GetProvider().CreateAsyncScope();
        var profileQueryRepository = scope.ServiceProvider.GetRequiredService<IProfileQueryRepository>();
        var profile = await profileQueryRepository
            .WithResume()
            .WithTalents()
            .WithTestimonials()
            .WithSkills()
            .WithProject()
            .WhereCondition(x => x.Id == 1)
            .BuildFirstOrDefaultAsync();

        profile.ShouldNotBeNull();
        profile.Resume.ShouldNotBeNull();
        profile.Resume.ContactInfo.ShouldNotBeNull();
        profile.SkillSet.Skills.Count.ShouldBe(SkillInfoCollection.Data.Count);
        profile.Talents.Count.ShouldBe(new TalentsCollection().GetTalents().Count);
        profile.Testimonials.Count.ShouldBe(new TestimonialCollection().GetTestimonials().Count);
        profile.Projects.Count.ShouldBe(new ProjectCollection().GetProjects().Count);
    }
}