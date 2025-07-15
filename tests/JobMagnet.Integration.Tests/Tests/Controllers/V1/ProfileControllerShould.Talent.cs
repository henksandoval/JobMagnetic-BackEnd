using System.Net;
using System.Net.Http.Json;
using AutoFixture;
using AwesomeAssertions;
using JobMagnet.Application.Contracts.Commands.Talent;
using JobMagnet.Application.Contracts.Responses.Base;
using JobMagnet.Application.Contracts.Responses.TalentShowcase;
using JobMagnet.Domain.Aggregates.Profiles.Entities;
using JobMagnet.Application.Mappers;
using JobMagnet.Domain.Aggregates.Profiles.ValueObjects;
using JobMagnet.Domain.Ports.Repositories;
using JobMagnet.Domain.Ports.Repositories.Base;
using JobMagnet.Shared.Tests.Utils;
using Microsoft.Extensions.DependencyInjection;

namespace JobMagnet.Integration.Tests.Tests.Controllers.V1;

public partial class ProfileControllerShould
{
    [Fact(DisplayName = "Should return 201 Created with the new talents when talents are added to a profile")]
    public async Task AddTalentsToProfileAsync()
    {
        // --- Given ---
        var profile = await SetupProfileAsync();
        var talentsData = GetTalentBase(profile.Id.Value);
        var createdTalents = _fixture.Build<TalentCommand>()
            .With(t => t.TalentData, talentsData)
            .Create();
        var httpContent = TestUtilities.SerializeRequestContent(createdTalents);

        // --- When ---
        var response = await _httpClient.PostAsync($"{RequestUriController}/{profile.Id.Value}/talent", httpContent);

        // --- Then ---
        response.IsSuccessStatusCode.Should().BeTrue();
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var locationHeader = response.Headers.Location!.ToString();
        locationHeader.Should().NotBeNull();
        var expectedHeader = $"{RequestUriController}/{profile.Id.Value}/talent";
        locationHeader.Should().Match(currentHeader =>
            currentHeader.Contains(expectedHeader, StringComparison.OrdinalIgnoreCase)
        );

        var responseData = await TestUtilities.DeserializeResponseAsync<TalentResponse>(response);
        responseData.Should().NotBeNull();

        await using var scope = _testFixture.GetProvider().CreateAsyncScope();
        var queryRepository = scope.ServiceProvider.GetRequiredService<IProfileQueryRepository>();
        var entityCreated = await queryRepository
            .WithTalents()
            .GetByIdAsync(profile.Id, CancellationToken.None);
        entityCreated.Should().NotBeNull();
        entityCreated.TalentShowcase.Should().NotBeNull();
        entityCreated.TalentShowcase.Should().HaveCount(1);
        var createdTalent = entityCreated.TalentShowcase.First();
        createdTalent.ProfileId.Value.Should().Be(talentsData.ProfileId);
        createdTalent.Description.Should().Be(talentsData.Description);
    }

    [Fact(DisplayName = "Should return 200 OK with a list of talents when the profile exists and has talents")]
    public async Task GetTalents_WhenProfileExistsAndHasTalents()
    {
        // --- Given ---
        var profileWithTalents = await SetupProfileAsync();
        var expectedTalents = profileWithTalents.TalentShowcase.Select(p => p.ToModel());
        
        // --- When ---
        var response = await _httpClient.GetAsync($"{RequestUriController}/{profileWithTalents.Id.Value}/talents");
            
        // --- Then ---
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var responseData = await TestUtilities.DeserializeResponseAsync<List<TalentResponse>>(response);
        responseData.Should().NotBeNull();
        responseData.Should().BeEquivalentTo(expectedTalents);
    }

    [Fact(DisplayName = "Should return 404 Not Found when the profile does not exist")]
    public async Task GetTalents_WhenProfileDoesNotExist()
    {
        // --- Given ---
        var nonExistentProfileId = Guid.NewGuid();
        await _testFixture.ResetDatabaseAsync();
        // --- When ---
        var response = await _httpClient.GetAsync($"{RequestUriController}/{nonExistentProfileId}/talents");
        
        // --- Then ---
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    [Fact(DisplayName = "Should return 204 ok when updating an existing project")]
    public async Task UpdateTalents_WhenProfileExistsAndHasTalents()
    {
        // --- Given ---
        _talentsCount = 4;
        var profile = await SetupProfileAsync();
        var updatedTalentData = GetTalentBase(profile.Id.Value);
        var talentToUpdate = profile.TalentShowcase.First();
        var updateCommand = _fixture.Build<TalentCommand>()
            .With(t => t.TalentData, updatedTalentData)
            .Create();
        var responseuri = $"{RequestUriController}/{profile.Id.Value}/talents/{talentToUpdate.Id.Value}";

        // --- When ---
        var response = await _httpClient.PutAsJsonAsync(responseuri, updateCommand);
        
        // --- Then ---
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        var talentUpdated = await FindTalentByIdAsync(talentToUpdate.Id.Value);
        talentUpdated.Should().NotBeNull();
        talentUpdated.ProfileId.Should().Be(profile.Id);
        var commandBase = updateCommand.TalentData;
        talentUpdated.Should().BeEquivalentTo(commandBase, options => options
            .Excluding(expect => expect!.ProfileId)
        );
    }
    
    [Fact(DisplayName = "Should return 404 Not Found when the profileId does not exist for an update")]
    public async Task UpdateTalent_WhenProfileDoesNotExist()
    {
        // --- Given ---
        var nonExistentProfileId = Guid.NewGuid();
        var talentData = GetTalentBase(nonExistentProfileId);
        var requestUri = $"{RequestUriController}/{nonExistentProfileId}/talents/{nonExistentProfileId}";

        // --- When ---
        var response = await _httpClient.PutAsJsonAsync(requestUri, talentData);

        // --- Then ---
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    [Fact(DisplayName = "Should return 204 No Content when deleting an existing talent")]
    public async Task DeleteTalent_WhenProfileExistsAndHasTalents()
    {
        // --- Given ---
        _talentsCount = 4;
        var profile = await SetupProfileAsync();
        var talentToDelete = profile.TalentShowcase.First();
        var requestUri = $"{RequestUriController}/{profile.Id.Value}/talents/{talentToDelete.Id.Value}";

        // --- When ---
        var response = await _httpClient.DeleteAsync(requestUri);

        // --- Then ---
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        await using var scope = _testFixture.GetProvider().CreateAsyncScope();
        var queryRepository = scope.ServiceProvider.GetRequiredService<IQueryRepository<Talent, TalentId>>();
        var talents = await queryRepository.FindAsync(t => t.ProfileId == talentToDelete.ProfileId , CancellationToken.None);
        talents.Should().HaveCount(_talentsCount - 1);
        talents.Should().NotContain(t => t.Id == talentToDelete.Id);
    }
    
    [Fact(DisplayName = "Should return 404 Not Found when deleting a talent that does not exist")]
    public async Task DeleteTalent_WhenTalentDoesNotExist()
    {
        // --- Given ---
        var nonExistentProfileId = Guid.NewGuid();
        var requestUri = $"{RequestUriController}/{nonExistentProfileId}/talents/{nonExistentProfileId}";

        // --- When ---
        var response = await _httpClient.DeleteAsync(requestUri);

        // --- Then ---
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    
    
    private TalentBase GetTalentBase(Guid profileEntityId)
    {
        return _fixture
            .Build<TalentBase>()
            .With(t => t.ProfileId, profileEntityId)
            .Create();
    }
    
    private async Task<Talent?> FindTalentByIdAsync(Guid id)
    {
        await using var scope = _testFixture.GetProvider().CreateAsyncScope();
        var queryRepository = scope.ServiceProvider.GetRequiredService<IQueryRepository<Talent, TalentId>>();
        var entityUpdated = await queryRepository.GetByIdAsync(new TalentId(id), CancellationToken.None);
        return entityUpdated;
    }
}