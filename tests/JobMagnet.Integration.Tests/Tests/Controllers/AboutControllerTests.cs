using System.Net;
using AutoFixture;
using FluentAssertions;
using JobMagnet.Entities;
using JobMagnet.Integration.Tests.Fixtures;
using JobMagnet.Integration.Tests.Utils;
using JobMagnet.Models;
using JobMagnet.Repository.Interface;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace JobMagnet.Integration.Tests.Tests.Controllers;

public class AboutControllerTests : IClassFixture<JobMagnetTestSetupFixture>
{
    private const string RequestUriController = "api/about";
    private readonly JobMagnetTestSetupFixture _testFixture;
    private readonly HttpClient _httpClient;
    private readonly Fixture _fixture = new();

    public AboutControllerTests(JobMagnetTestSetupFixture testFixture)
    {
        _testFixture = testFixture;
        _httpClient = _testFixture.GetClient();
    }

    [Fact(DisplayName = "Should return the record and return 200 when a valid ID is provided")]
    public async Task ShouldReturnAboutRecord_WhenValidIdIsProvidedAsync()
    {
        var aboutEntity = await CreateAndPersistAboutEntityAsync();

        var response = await _httpClient.GetAsync($"{RequestUriController}/{aboutEntity.Id}");

        response.IsSuccessStatusCode.ShouldBeTrue();
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var responseData = await TestUtilities.DeserializeResponseAsync<AboutModel>(response);
        responseData.ShouldNotBeNull();
        responseData.Should().BeEquivalentTo(aboutEntity, options => options.ExcludingMissingMembers());
    }

    [Fact(DisplayName = "Should create a new About and return 201 when the request is valid")]
    public async Task ShouldReturnCreatedAndPersistData_WhenRequestIsValidAsync()
    {
        var createRequest = _fixture.Build<AboutCreateRequest>().Create();
        var httpContent = TestUtilities.SerializeRequestContent(createRequest);

        var response = await _httpClient.PostAsync(RequestUriController, httpContent);

        response.IsSuccessStatusCode.ShouldBeTrue();
        response.StatusCode.ShouldBe(HttpStatusCode.Created);

        var responseData = await TestUtilities.DeserializeResponseAsync<AboutModel>(response);
        responseData.ShouldNotBeNull();

        var locationHeader = response.Headers.Location!.ToString();
        locationHeader.ShouldNotBeNull();
        locationHeader.ShouldContain($"{RequestUriController}/{responseData.Id}");

        await using var scope = _testFixture.GetProvider().CreateAsyncScope();
        var aboutRepository = scope.ServiceProvider.GetRequiredService<IAboutRepository<AboutEntity>>();
        var aboutCreated = await aboutRepository.GetByIdAsync(responseData.Id);

        aboutCreated.ShouldNotBeNull();
        aboutCreated.Should().BeEquivalentTo(createRequest, options => options.ExcludingMissingMembers());
    }

    private async Task<AboutEntity> CreateAndPersistAboutEntityAsync()
    {
        await using var scope = _testFixture.GetProvider().CreateAsyncScope();
        var aboutRepository = scope.ServiceProvider.GetRequiredService<IAboutRepository<AboutEntity>>();

        var aboutEntity = _fixture.Build<AboutEntity>().With(x => x.Id, 0).Create();
        await aboutRepository.CreateAsync(aboutEntity);

        return aboutEntity;
    }
}