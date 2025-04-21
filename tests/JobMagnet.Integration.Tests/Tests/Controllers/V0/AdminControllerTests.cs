using System.Net;
using JobMagnet.Infrastructure.Context;
using JobMagnet.Integration.Tests.Fixtures;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit.Abstractions;

namespace JobMagnet.Integration.Tests.Tests.Controllers.V0;

public class AdminControllerTests(JobMagnetTestEmptyDatabaseSetupFixture testFixture, ITestOutputHelper testOutputHelper)
    : IClassFixture<JobMagnetTestEmptyDatabaseSetupFixture>
{
    private const string RequestUriController = "api/v0.1/admin";
    private readonly HttpClient _httpClient = testFixture.GetClient();
    private readonly ITestOutputHelper _testOutputHelper = testFixture.SetTestOutputHelper(testOutputHelper);

    [Fact(DisplayName = "Should return 200 and Pong when GET ping request is called")]
    public async Task ShouldReturnPong_WhenGetPingRequestAsync()
    {
        // When
        var response = await _httpClient.GetAsync($"{RequestUriController}/ping");

        // Then
        response.IsSuccessStatusCode.ShouldBeTrue();
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var responseData = await response.Content.ReadAsStringAsync();
        responseData.ShouldNotBeNullOrEmpty();
        responseData.ShouldBe("Pong");
    }

    [Fact(DisplayName = "Should delete and return 204 when DELETE request is received")]
    public async Task ShouldDestroyDatabase_WhenDeleteRequestIsReceivedIsAsync()
    {
        // Given
        await using var scope = testFixture.GetProvider().CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<JobMagnetDbContext>();
        await dbContext.Database.EnsureCreatedAsync();
        var canConnect = await dbContext.Database.CanConnectAsync();
        _testOutputHelper.WriteLine("Database connection status: {0}", canConnect);

        // When
        var response = await _httpClient.DeleteAsync($"{RequestUriController}");

        // Then
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);

        canConnect = await dbContext.Database.CanConnectAsync();
        canConnect.ShouldBeFalse();
    }

    [Fact(DisplayName = "Should Create and return 200 when Post request is received")]
    public async Task ShouldCreateDatabase_WhenPostRequestIsReceivedIsAsync()
    {
        // When
        var response = await _httpClient.PostAsync($"{RequestUriController}", null);

        // Then
        response.IsSuccessStatusCode.ShouldBeTrue();
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        await using var scope = testFixture.GetProvider().CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<JobMagnetDbContext>();
        var canConnect = await dbContext.Database.CanConnectAsync();
        canConnect.ShouldBeTrue();
    }
}