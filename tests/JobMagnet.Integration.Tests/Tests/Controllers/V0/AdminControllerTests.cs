using System.Net;
using JobMagnet.Integration.Tests.Fixtures;
using Shouldly;
using Xunit.Abstractions;

namespace JobMagnet.Integration.Tests.Tests.Controllers.V0;

public class AdminControllerTests : IClassFixture<JobMagnetTestSetupFixture>
{
    private const string RequestUriController = "api/v0.1/admin";
    private readonly HttpClient _httpClient;

    public AdminControllerTests(JobMagnetTestSetupFixture testFixture, ITestOutputHelper testOutputHelper)
    {
        _httpClient = testFixture.GetClient();
        testFixture.SetTestOutputHelper(testOutputHelper);
    }

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
}