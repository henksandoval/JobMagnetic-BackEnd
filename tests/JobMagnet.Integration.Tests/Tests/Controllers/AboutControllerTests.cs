using System.Text;
using AutoFixture;
using JobMagnet.Context;
using JobMagnet.Integration.Tests.Fixtures;
using JobMagnet.Models;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace JobMagnet.Integration.Tests.Tests.Controllers;

public class AboutControllerTests : IClassFixture<JobMagnetTestSetupFixture>
{
    private readonly JobMagnetTestSetupFixture _jobMagnetTestSetupFixture;

    public AboutControllerTests(JobMagnetTestSetupFixture jobMagnetTestSetupFixture)
    {
        _jobMagnetTestSetupFixture = jobMagnetTestSetupFixture;
    }

    [Fact(DisplayName = "When about endpoint is called with PostRequest, it should register a About")]
    public async Task CreateAboutAsync()
    {
        // Initialize
        await using var scope = _jobMagnetTestSetupFixture.GetProvider().CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<JobMagnetDbContext>();
        var fixture = new Fixture();

        // Act
        var createRequest = fixture.Build<AboutCreateRequest>().Create();
        var httpContent = new StringContent(JsonConvert.SerializeObject(createRequest), Encoding.UTF8, "application/json");
        var httpClient = _jobMagnetTestSetupFixture.GetClient();
        var response = await httpClient.PostAsync($"api/about", httpContent);

        // Assert
        Assert.True(response.IsSuccessStatusCode);
    }
}