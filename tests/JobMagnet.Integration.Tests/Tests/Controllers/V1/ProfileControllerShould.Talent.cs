using System.Net;
using System.Net.Http.Json;
using AutoFixture;
using AwesomeAssertions;
using JobMagnet.Application.Contracts.Commands.Talent;
using JobMagnet.Application.Contracts.Responses.Base;
using JobMagnet.Domain.Aggregates.Profiles.Entities;
using JobMagnet.Shared.Tests.Utils;
using JobMagnet.Shared.Utils;

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
        var response = await _httpClient.PostAsync($"{RequestUriController}/{profile.Id.Value}/talents", httpContent);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var expecteTalent = await response.Content.ReadFromJsonAsync<List<Talent>>();
        createdTalents.Should().NotBeNull();
        createdTalents.Should().Be(expecteTalent!.Count);
    }
    
    
    private TalentBase GetTalentBase(Guid profileEntityId)
    {
        return _fixture
            .Build<TalentBase>()
            .With(t => t.ProfileId, profileEntityId)
            .Create();
    }
}