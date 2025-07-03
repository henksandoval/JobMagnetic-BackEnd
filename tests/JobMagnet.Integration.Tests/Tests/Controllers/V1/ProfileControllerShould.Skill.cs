using System.Net;
using System.Net.Http.Json;
using AutoFixture;
using AwesomeAssertions;
using JobMagnet.Application.Contracts.Commands.Skill;
using JobMagnet.Application.Contracts.Responses.Base;
using JobMagnet.Application.Contracts.Responses.Skill;
using JobMagnet.Application.Mappers;
using JobMagnet.Domain.Aggregates.Profiles;
using JobMagnet.Domain.Aggregates.Profiles.Entities;
using JobMagnet.Domain.Ports.Repositories;
using JobMagnet.Domain.Ports.Repositories.Base;
using JobMagnet.Shared.Tests.Fixtures;
using JobMagnet.Shared.Tests.Utils;
using Microsoft.Extensions.DependencyInjection;

namespace JobMagnet.Integration.Tests.Tests.Controllers.V1;

public partial class ProfileControllerShould
{
    [Trait("Profile", "Skills")]
    [Fact(DisplayName = "Should return 201 Created with the new SkillSet when payload is valid for an existing profile")]
    public async Task AddSkillSet_WhenProfileExistsAndPayloadIsValid()
    {
        // --- Given ---
        _loadSkillSet = false;
        _skillsCount = 0;
        var profile = await SetupProfileAsync();
        var skillSetData = GetSkillSetData(profile.Id.Value,5);
        var skillNotRegistered = new SkillBase
        {
            Name = "Cunnilingus",
            ProficiencyLevel = (ushort) FixtureBuilder.Faker.Random.Short(0, 10)
        };
        skillSetData.Skills.Add(skillNotRegistered);
        var createRequest = _fixture.Build<SkillCommand>()
            .With(x => x.SkillSetData, skillSetData)
            .Create();
        var httpContent = TestUtilities.SerializeRequestContent(createRequest);

        // --- When ---
        var response = await _httpClient.PostAsync($"{RequestUriController}/{profile.Id.Value}/skills", httpContent);

        // --- Then ---
        response.IsSuccessStatusCode.Should().BeTrue();
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var responseData = await TestUtilities.DeserializeResponseAsync<SkillResponse>(response);
        responseData.Should().NotBeNull();

        var locationHeader = response.Headers.Location!.ToString();
        locationHeader.Should().NotBeNull();
        var expectedHeader = $"{RequestUriController}/{profile.Id.Value}/skills";
        locationHeader.Should().Match(currentHeader =>
            currentHeader.Contains(expectedHeader, StringComparison.OrdinalIgnoreCase)
        );

        var entityCreated = await FindSkillSetByIdAsync(profile.Id.Value);

        entityCreated.Should().NotBeNull();
        entityCreated.ProfileId.Should().Be(profile.Id);
        entityCreated.Skills.Should().HaveSameCount(createRequest.SkillSetData.Skills);
    }

    [Trait("Profile", "Skills")]
    [Fact(DisplayName = "Should return 200 OK with a list of SkillSets when the profile exists and has SkillSets")]
    public async Task GetSkillSets_WhenProfileExistsAndHasSkillSets()
    {
        // --- Given ---
        var profileWithSkillSets = await SetupProfileAsync();
        var expectedSkillSets = profileWithSkillSets.SkillSet!.ToModel();

        // --- When ---
        var response = await _httpClient.GetAsync($"{RequestUriController}/{profileWithSkillSets.Id.Value}/skills");

        // --- Then ---
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var responseData = await TestUtilities.DeserializeResponseAsync<List<SkillResponse>>(response);
        responseData.Should().NotBeNull();
    }

    [Trait("Profile", "Skills")]
    [Fact(DisplayName = "Should return 404 Not Found when the profile ID does not exist")]
    public async Task GetSkillsNotFound_WhenProfileDoesNotExist()
    {
        // --- Given ---
        var nonExistentProfileId = Guid.NewGuid();
        await _testFixture.ResetDatabaseAsync();

        // --- When ---
        var response = await _httpClient.GetAsync($"{RequestUriController}/{nonExistentProfileId}/skills");

        // --- Then ---
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Trait("Profile", "Skills")]
    [Fact(DisplayName = "Should return 200 OK with an empty list when the profile exists but has no SkillSets")]
    public async Task GetOkAndEmptyList_WhenProfileExistsButHasNoSkillSets()
    {
        // --- Given ---
        _skillsCount = 0;
        var profileWithoutSkillSets = await SetupProfileAsync();

        // --- When ---
        var response = await _httpClient.GetAsync($"{RequestUriController}/{profileWithoutSkillSets.Id.Value}/skills");

        // --- Then ---
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var responseData = await TestUtilities.DeserializeResponseAsync<List<SkillResponse>>(response);
        responseData.Should().NotBeNull().And.BeEmpty();
    }

    [Trait("Profile", "Skills")]
    [Fact(DisplayName = "Should return 204 No Content when updating an existing SkillSet")]
    public async Task UpdateSkillSet_WhenProfileAndSkillSetExistAndPayloadIsValid()
    {
        // --- Given ---
        _skillsCount = 8;
        var profile = await SetupProfileAsync();
        var skillSetData = GetSkillSetData(profile.Id.Value);
        var skillSetToUpdate = profile.SkillSet!.Skills.First();
        var command = _fixture.Build<SkillCommand>()
            .With(x => x.SkillSetData, skillSetData)
            .Create();
        var requestUri = $"{RequestUriController}/{profile.Id.Value}/skills/{skillSetToUpdate.Id.Value}";

        // --- When ---
        var response = await _httpClient.PutAsJsonAsync(requestUri, command);

        // --- Then ---
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        var skillSetUpdated = await FindSkillSetByIdAsync(skillSetToUpdate.Id.Value);

        skillSetUpdated.Should().NotBeNull();
        skillSetUpdated.ProfileId.Should().Be(profile.Id);
        var commandBase = command.SkillSetData;
        skillSetUpdated.Should().BeEquivalentTo(commandBase, options => options
            .Excluding(expect => expect!.ProfileId)
        );
    }

    [Trait("Profile", "Skills")]
    [Fact(DisplayName = "Should return 404 Not Found when the profile ID does not exist for an update")]
    public async Task UpdateSkillSet_WhenProfileDoesNotExist()
    {
        // --- Given ---
        var nonExistentProfileId = Guid.NewGuid();
        var skillSetData = GetSkillSetData(nonExistentProfileId);
        var requestUri = $"{RequestUriController}/{nonExistentProfileId}/skills/{nonExistentProfileId}";

        // --- When ---
        var response = await _httpClient.PutAsJsonAsync(requestUri, skillSetData);

        // --- Then ---
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Trait("Profile", "Skills")]
    [Fact(DisplayName = "Should return 404 Not Found when the SkillSet does not belong to the profile")]
    public async Task UpdateSkillSet_WhenSkillSetDoesNotExistInProfile()
    {
        // --- Given ---
        var profile = await SetupProfileAsync();
        var skillSetData = GetSkillSetData(profile.Id.Value);
        var nonExistentSkillSetId = Guid.NewGuid();
        var command = _fixture.Build<SkillCommand>()
            .With(x => x.SkillSetData, skillSetData)
            .Create();
        var requestUri = $"{RequestUriController}/{profile.Id.Value}/skills/{nonExistentSkillSetId}";

        // --- When ---
        var response = await _httpClient.PutAsJsonAsync(requestUri, command);

        // --- Then ---
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Trait("Profile", "Skills")]
    [Fact(DisplayName = "Should return 204 No Content when deleting an existing SkillSet")]
    public async Task DeleteSkillSet_WhenProfileAndSkillSetExist()
    {
        // --- Given ---
        _skillsCount = 5;
        var profile = await SetupProfileAsync();
        var skillSetToDelete = profile.SkillSet;
        var requestUri = $"{RequestUriController}/{profile.Id.Value}/skills/{skillSetToDelete!.Id.Value}";

        // --- When ---
        var response = await _httpClient.DeleteAsync(requestUri);

        // --- Then ---
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        await using var scope = _testFixture.GetProvider().CreateAsyncScope();
        var queryRepository = scope.ServiceProvider.GetRequiredService<IQueryRepository<SkillSet, SkillSetId>>();
        var skillSets = await queryRepository.FindAsync(p => p.ProfileId == skillSetToDelete.ProfileId, CancellationToken.None);

        skillSets.Should().HaveCount(_skillsCount - 1);
        skillSets.Should().NotContain(p => p.Id == skillSetToDelete.Id);
    }

    [Trait("Profile", "Skills")]
    [Fact(DisplayName = "Should return 404 Not Found when the profile ID does not exist for a delete")]
    public async Task DeleteSkillSet_WhenProfileDoesNotExist()
    {
        // --- Given ---
        var nonExistentProfileId = Guid.NewGuid();
        var requestUri = $"{RequestUriController}/{nonExistentProfileId}/skills/{Guid.NewGuid()}";

        // --- When ---
        var response = await _httpClient.DeleteAsync(requestUri);

        // --- Then ---
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Trait("Profile", "Skills")]
    [Fact(DisplayName = "Should return 404 Not Found when deleting a SkillSet that does not belong to the profile")]
    public async Task DeleteSkillSet_WhenSkillSetDoesNotExistInProfile()
    {
        // --- Given ---
        var profile = await SetupProfileAsync();
        var nonExistentSkillSetId = Guid.NewGuid();
        var requestUri = $"{RequestUriController}/{profile.Id.Value}/skills/{nonExistentSkillSetId}";

        // --- When ---
        var response = await _httpClient.DeleteAsync(requestUri);

        // --- Then ---
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    private SkillSetBase GetSkillSetData(Guid profileEntityId, int skillsQuantity = 3)
    {
        var skills = _fixture.CreateMany<SkillBase>(skillsQuantity).ToList();

        return _fixture
            .Build<SkillSetBase>()
            .With(x => x.ProfileId, profileEntityId)
            .With(x => x.Skills, skills)
            .Create();
    }

    private async Task<SkillSet?> FindSkillSetByIdAsync(Guid id)
    {
        await using var scope = _testFixture.GetProvider().CreateAsyncScope();
        var queryRepository = scope.ServiceProvider.GetRequiredService<IProfileQueryRepository>();
        var entityCreated = await queryRepository
            .WithSkills()
            .WhereCondition(x => x.Id == new ProfileId(id))
            .BuildFirstOrDefaultAsync(CancellationToken.None, true);

        return entityCreated!.SkillSet;
    }
}