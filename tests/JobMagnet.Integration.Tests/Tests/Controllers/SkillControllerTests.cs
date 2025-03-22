using System.Net;
using AutoFixture;
using FluentAssertions;
using JobMagnet.Entities;
using JobMagnet.Integration.Tests.Fixtures;
using JobMagnet.Integration.Tests.Utils;
using JobMagnet.Models;
using JobMagnet.Repositories.Interface;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit.Abstractions;

namespace JobMagnet.Integration.Tests.Tests.Controllers;

public class SkillControllerTests : IClassFixture<JobMagnetTestSetupFixture>
{
    private const string RequestUriController = "api/skill";
    private readonly Fixture _fixture = new();
    private readonly HttpClient _httpClient;
    private readonly JobMagnetTestSetupFixture _testFixture;
    private readonly ITestOutputHelper _testOutputHelper;

    public SkillControllerTests(JobMagnetTestSetupFixture testFixture, ITestOutputHelper testOutputHelper)
    {
        _testFixture = testFixture;
        _testOutputHelper = testOutputHelper;
        _httpClient = _testFixture.GetClient();
        _testFixture.SetTestOutputHelper(testOutputHelper);
    }

    [Fact(DisplayName = "Should create a new record and return 201 when the request is valid")]
    public async Task ShouldReturnCreatedAndPersistData_WhenRequestIsValidAsync()
    {
        await _testFixture.ResetDatabaseAsync();
        _testOutputHelper.WriteLine("Executing test: {0} in time: {1}",
            nameof(ShouldReturnCreatedAndPersistData_WhenRequestIsValidAsync), DateTime.Now);
        var createRequest = _fixture.Build<SkillCreateRequest>().Create();
        var httpContent = TestUtilities.SerializeRequestContent(createRequest);

        var response = await _httpClient.PostAsync(RequestUriController, httpContent);

        response.IsSuccessStatusCode.ShouldBeTrue();
        response.StatusCode.ShouldBe(HttpStatusCode.Created);

        var responseData = await TestUtilities.DeserializeResponseAsync<SkillModel>(response);
        responseData.ShouldNotBeNull();

        var locationHeader = response.Headers.Location!.ToString();
        locationHeader.ShouldNotBeNull();
        locationHeader.ShouldContain($"{RequestUriController}/{responseData.Id}");

        await using var scope = _testFixture.GetProvider().CreateAsyncScope();
        var queryRepository = scope.ServiceProvider.GetRequiredService<IQueryRepository<SkillEntity>>();
        var entityCreated = await queryRepository.GetByIdAsync(responseData.Id);

        entityCreated.ShouldNotBeNull();
        entityCreated.Should().BeEquivalentTo(createRequest, options => options.ExcludingMissingMembers());
    }

    private async Task<SkillEntity> CreateAndPersistEntityAsync()
    {
        await using var scope = _testFixture.GetProvider().CreateAsyncScope();
        var commandRepository = scope.ServiceProvider.GetRequiredService<ICommandRepository<SkillEntity>>();

        var entity = _fixture.Build<SkillEntity>().With(x => x.Id, 0).Create();
        await commandRepository.CreateAsync(entity);

        return entity;
    }
}