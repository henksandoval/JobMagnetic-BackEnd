using System.Net;
using AutoFixture;
using AwesomeAssertions;
using JobMagnet.Application.Contracts.Commands.Talent;
using JobMagnet.Application.Contracts.Responses.Base;
using JobMagnet.Application.Contracts.Responses.TalentShowcase;
using JobMagnet.Application.Mappers;
using JobMagnet.Domain.Ports.Repositories;
using JobMagnet.Shared.Tests.Utils;
using Microsoft.Extensions.DependencyInjection;

namespace JobMagnet.Integration.Tests.Tests.Controllers.V1;

public partial class ProfileControllerShould
{
    [Fact(DisplayName = "Should return 201 Created with the new talents when talents are added to a profile")]
    public async Task AddTalentsToProfileAsync()
    {
        // Arrange
        var profile = await SetupProfileAsync();
        var talentsData = GetTalentBase(profile.Id.Value);
        var createdTalents = _fixture.Build<TalentCommand>()
            .With(t => t.TalentData, talentsData)
            .Create();
        var httpContent = TestUtilities.SerializeRequestContent(createdTalents);

        // Act
        var response = await _httpClient.PostAsync($"{RequestUriController}/{profile.Id.Value}/talent", httpContent);

        // Assert
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
        entityCreated.Talents.Should().NotBeNull();
        entityCreated.Talents.Should().HaveCount(1);
        var createdTalent = entityCreated.Talents.First();
        createdTalent.ProfileId.Value.Should().Be(talentsData.ProfileId);
        createdTalent.Description.Should().Be(talentsData.Description);
    }
    
    
    private TalentBase GetTalentBase(Guid profileEntityId)
    {
        return _fixture
            .Build<TalentBase>()
            .With(t => t.ProfileId, profileEntityId)
            .Create();
    }
    

    [Fact(DisplayName = "Should return 200 OK with a list of talents when the profile exists and has talents")]
    public async Task GetTalents_WhenProfileExistsAndHasTalents()
    {
        // --- Given ---
        var profileWithTalents = await SetupProfileAsync();
        var expectedTalents = profileWithTalents.Talents.Select(p => p.ToModel());
        
        // Act
        var response = await _httpClient.GetAsync($"{RequestUriController}/{profileWithTalents.Id.Value}/talents");
            
        // assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var responseData = await TestUtilities.DeserializeResponseAsync<List<TalentResponse>>(response);
        responseData.Should().NotBeNull();
        responseData.Should().BeEquivalentTo(expectedTalents);
    }
    
    // private async Task<Talent?> FindTalentByIdAsync(Guid id)
    // {
    //     await using var scope = _testFixture.GetProvider().CreateAsyncScope();
    //     var queryRepository = scope.ServiceProvider.GetRequiredService<IProfileQueryRepository>();
    //     var entityCreated = await queryRepository
    //         .WithTalents()
    //         .GetByIdAsync(profile.Id, CancellationToken.None);
    //     return entityCreated;
    // }
    
}