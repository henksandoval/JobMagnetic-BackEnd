using System.Net;
using System.Net.Http.Json;
using AutoFixture;
using AwesomeAssertions;
using JobMagnet.Application.Contracts.Commands.Skill;
using JobMagnet.Application.Contracts.Responses.Base;
using JobMagnet.Application.Contracts.Responses.Skill;
using JobMagnet.Application.Mappers;
using JobMagnet.Domain.Aggregates.Profiles.Entities;
using JobMagnet.Domain.Aggregates.Profiles.ValueObjects;
using JobMagnet.Domain.Ports.Repositories;
using JobMagnet.Infrastructure.Persistence.Context;
using JobMagnet.Shared.Tests.Fixtures;
using JobMagnet.Shared.Tests.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace JobMagnet.Integration.Tests.Tests.Controllers.V1;

public partial class ProfileControllerShould
{
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

        var entityCreated = await FindSkillSetByIdAsync(profile.Id);

        entityCreated.Should().NotBeNull();
        entityCreated.ProfileId.Should().Be(profile.Id);
        entityCreated.Skills.Should().HaveSameCount(createRequest.SkillSetData.Skills);
    }

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

    [Fact(DisplayName = "Should return 204 No Content when updating an existing SkillSet")]
    public async Task UpdateSkillSet_WhenProfileAndSkillSetExistAndPayloadIsValid()
    {
        // --- Given ---
        _skillsCount = 8;
        var profile = await SetupProfileAsync();
        var skillSetToUpdate = profile.GetSkills().First();
        var newProficiencyLevel = (ushort) (skillSetToUpdate.ProficiencyLevel + 1 >= Skill.MaximumProficiencyLevel
            ? skillSetToUpdate.ProficiencyLevel - 1
            : skillSetToUpdate.ProficiencyLevel + 1);
        var skillsToUpdate = new List<SkillBase>
        {
            new() { Id = skillSetToUpdate.Id.Value, ProficiencyLevel = newProficiencyLevel }
        };
        var skillSetData = _fixture.Build<SkillSetBase>()
            .With(s => s.Skills, skillsToUpdate)
            .Create();
        var command = _fixture.Build<SkillCommand>()
            .With(c => c.SkillSetData, skillSetData)
            .Create();

        var requestUri = $"{RequestUriController}/{profile.Id.Value}/skills/{skillSetToUpdate.Id.Value}";

        // --- When ---
        var response = await _httpClient.PutAsJsonAsync(requestUri, command);

        // --- Then ---
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        var entityUpdated = await FindSkillSetByIdAsync(profile.Id);

        entityUpdated.Should().NotBeNull();
        entityUpdated.ProfileId.Should().Be(profile.Id);
        var commandBase = command.SkillSetData;
        entityUpdated.Skills.Should().HaveSameCount(profile.GetSkills());
        var skillUpdated = entityUpdated.Skills.First(x => x.Id == skillSetToUpdate.Id);
        skillUpdated.ProficiencyLevel.Should().Be(newProficiencyLevel);
    }

    [Fact(DisplayName = "Should return 404 Not Found when the profile ID does not exist for an update")]
    public async Task UpdateSkillSet_WhenProfileDoesNotExist()
    {
        // --- Given ---
        var nonExistentProfileId = Guid.NewGuid();
        var command = _fixture.Create<SkillCommand>();
        var requestUri = $"{RequestUriController}/{nonExistentProfileId}/skills/{nonExistentProfileId}";

        // --- When ---
        var response = await _httpClient.PutAsJsonAsync(requestUri, command);

        // --- Then ---
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

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

    [Fact(DisplayName = "Should return 204 No Content when delete an existing Skill to SkillSet")]
    public async Task DeleteSkill_WhenProfileAndSkillSetExist()
    {
        // --- Given ---
        _skillsCount = 5;
        var profile = await SetupProfileAsync();
        var skillToDelete = profile.GetSkills().FirstOrDefault();
        var requestUri = $"{RequestUriController}/{profile.Id.Value}/skills/{skillToDelete!.Id.Value}";

        // --- When ---
        var response = await _httpClient.DeleteAsync(requestUri);

        // --- Then ---
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        await using var scope = _testFixture.GetProvider().CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<JobMagnetDbContext>();
        var skillSets = await dbContext.SkillSets.Include(x => x.Skills)
            .FirstOrDefaultAsync(x => x.Id == profile.SkillSet.Id);

        skillSets.Should().NotBeNull();
        skillSets.Skills.Should().HaveCount(_skillsCount - 1);
        skillSets.Skills.Should().NotContain(p => p.Id == skillToDelete.Id);
    }

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

    [Fact(DisplayName = "Should return 204 No Content when arranging Skills with a valid order")]
    public async Task ArrangeSkills_WhenProfileExistsAndPayloadIsValid_ShouldUpdatePositionsInDb()
    {
        // --- Given ---
        _skillsCount = 5;
        var profile = await SetupProfileAsync();
        var skills = profile.GetSkills();
        var newOrderExpected = skills.OrderByDescending(x => x.Position).Select(x => x.Id).ToList();
        var newOrderCommand = newOrderExpected.Select(x => x.Value);

        var requestUri = $"{RequestUriController}/{profile.Id.Value}/Skills/arrange";

        // --- When ---
        var response = await _httpClient.PutAsJsonAsync(requestUri, newOrderCommand);

        // --- Then ---
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        var skillsUpdated = await FindSkillSetByIdAsync(profile.Id);
        var skillsOrderedByPosition = skillsUpdated!.Skills.OrderBy(p => p.Position).Select(x => x.Id);
        skillsOrderedByPosition.Should().BeEquivalentTo(newOrderExpected);
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

    private async Task<SkillSet?> FindSkillSetByIdAsync(ProfileId id)
    {
        await using var scope = _testFixture.GetProvider().CreateAsyncScope();
        var queryRepository = scope.ServiceProvider.GetRequiredService<IProfileQueryRepository>();
        var entityCreated = await queryRepository
            .WithSkills()
            .WhereCondition(x => x.Id == id)
            .BuildFirstOrDefaultAsync(CancellationToken.None, true);

        return entityCreated!.SkillSet;
    }
}