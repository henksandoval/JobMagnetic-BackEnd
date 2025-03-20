using System.Net;
using System.Net.Mime;
using System.Text;
using AutoFixture;
using FluentAssertions;
using JobMagnet.Entities;
using JobMagnet.Integration.Tests.Fixtures;
using JobMagnet.Models;
using JobMagnet.Repository.Interface;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Shouldly;

namespace JobMagnet.Integration.Tests.Tests.Controllers;

public class AboutControllerTests : IClassFixture<JobMagnetTestSetupFixture>
{
    private const string RequestUriController = "api/about";
    private readonly JobMagnetTestSetupFixture _jobMagnetTestSetupFixture;
    private readonly HttpClient _httpClient;
    private readonly Fixture _fixture = new();

    public AboutControllerTests(JobMagnetTestSetupFixture jobMagnetTestSetupFixture)
    {
        _jobMagnetTestSetupFixture = jobMagnetTestSetupFixture;
        _httpClient = _jobMagnetTestSetupFixture.GetClient();
    }

    [Fact(DisplayName = "Should create a new About and return 201 when the request is valid")]
    public async Task ShouldReturnCreatedAndPersistData_WhenRequestIsValidAsync()
    {
        // Arrange
        await using var scope = _jobMagnetTestSetupFixture.GetProvider().CreateAsyncScope();

        // Act
        var createRequest = _fixture.Build<AboutCreateRequest>().Create();
        var httpContent = new StringContent(JsonConvert.SerializeObject(createRequest), Encoding.UTF8,
            MediaTypeNames.Application.Json);
        var response = await _httpClient.PostAsync(RequestUriController, httpContent);

        // Assert
        var aboutRepository = scope.ServiceProvider.GetRequiredService<IAboutRepository<AboutEntity>>();
        var responseData = JsonConvert.DeserializeObject<AboutModel>(await response.Content.ReadAsStringAsync());

        response.IsSuccessStatusCode.ShouldBeTrue();
        response.StatusCode.ShouldBe(HttpStatusCode.Created);
        responseData.ShouldNotBeNull();

        var locationHeader = response.Headers!.Location!.ToString();
        locationHeader.ShouldNotBeNull();
        locationHeader.ShouldContain($"{RequestUriController}/{responseData.Id}");

        var aboutCreated = await aboutRepository.GetByIdAsync(responseData!.Id);
        aboutCreated.ShouldNotBeNull();
        aboutCreated.Should().BeEquivalentTo(createRequest, options => options.ExcludingMissingMembers());
    }

    [Fact(DisplayName = "Should return the record when a valid ID is provided")]
    public async Task ShouldReturnAboutRecord_WhenValidIdIsProvidedAsync()
    {
        // Arrange
        var fixture = new Fixture();
        await using var scope = _jobMagnetTestSetupFixture.GetProvider().CreateAsyncScope();

        var aboutRepository = scope.ServiceProvider.GetRequiredService<IAboutRepository<AboutEntity>>();
        var aboutEntity = fixture.Build<AboutEntity>().With(x => x.Id, 0).Create();
        await aboutRepository.CreateAsync(aboutEntity);

        // Act
        var httpClient = _jobMagnetTestSetupFixture.GetClient();
        var response = await httpClient.GetAsync($"{RequestUriController}/{aboutEntity.Id}");

        // Assert
        var responseData = JsonConvert.DeserializeObject<AboutModel>(await response.Content.ReadAsStringAsync());
        response.IsSuccessStatusCode.ShouldBeTrue();
        responseData.ShouldNotBeNull();
        responseData.Should().BeEquivalentTo(aboutEntity, options => options.ExcludingMissingMembers());
    }
}