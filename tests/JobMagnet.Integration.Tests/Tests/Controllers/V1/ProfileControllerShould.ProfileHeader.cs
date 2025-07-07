using System.Net;
using AutoFixture;
using AwesomeAssertions;
using JobMagnet.Application.Contracts.Commands.ProfileHeader;
using JobMagnet.Application.Contracts.Responses.Base;
using JobMagnet.Application.Contracts.Responses.ProfileHeader;
using JobMagnet.Domain.Aggregates.Profiles.Entities;
using JobMagnet.Domain.Aggregates.Profiles.ValueObjects;
using JobMagnet.Domain.Ports.Repositories;
using JobMagnet.Shared.Tests.Utils;
using Microsoft.Extensions.DependencyInjection;

namespace JobMagnet.Integration.Tests.Tests.Controllers.V1;

public partial class ProfileControllerShould
{
    [Fact(DisplayName = "Should return 201 Created with the new ProfileHeader when payload is valid for an existing profile")]
    public async Task AddProfileHeader_WhenProfileExistsAndPayloadIsValid()
    {
        // --- Given ---
        _loadHeader = false;
        _contactInfoCount = 0;
        var profile = await SetupProfileAsync();
        var profileHeaderData = GetProfileHeaderData(profile.Id.Value);
        var createRequest = _fixture.Build<ProfileHeaderCommand>()
            .With(x => x.ProfileHeaderData, profileHeaderData)
            .Create();
        var httpContent = TestUtilities.SerializeRequestContent(createRequest);

        // --- When ---
        var response = await _httpClient.PostAsync($"{RequestUriController}/{profile.Id.Value}/header", httpContent);

        // --- Then ---
        response.IsSuccessStatusCode.Should().BeTrue();
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var responseData = await TestUtilities.DeserializeResponseAsync<ProfileHeaderResponse>(response);
        responseData.Should().NotBeNull();

        var locationHeader = response.Headers.Location!.ToString();
        locationHeader.Should().NotBeNull();
        var expectedHeader = $"{RequestUriController}/{profile.Id.Value}/header";
        locationHeader.Should().Match(currentHeader =>
            currentHeader.Contains(expectedHeader, StringComparison.OrdinalIgnoreCase)
        );

        var entityCreated = await FindProfileHeaderByIdAsync(profile.Id);

        entityCreated.Should().NotBeNull();
        entityCreated.ProfileId.Should().Be(profile.Id);
        entityCreated.Should().BeEquivalentTo(profileHeaderData, options =>
            options.Excluding(x => x.ProfileId));
        entityCreated.ProfileId.Should().Be(profile.Id);
    }

    private ProfileHeaderBase GetProfileHeaderData(Guid idValue)
    {
        return _fixture.Build<ProfileHeaderBase>()
            .With(x => x.ProfileId, idValue)
            .Create();
    }
    /*
    [Fact(DisplayName = "Should return 200 OK with a list of ProfileHeader when the profile exists and has ProfileHeader")]
    public async Task GetProfileHeader_WhenProfileExistsAndHasProfileHeader()
    {
        // --- Given ---
        var profileWithProfileHeader = await SetupProfileAsync();
        var expectedProfileHeader = profileWithProfileHeader.ProfileHeader!.ToModel();

        // --- When ---
        var response = await _httpClient.GetAsync($"{RequestUriController}/{profileWithProfileHeader.Id.Value}/ProfileHeader");

        // --- Then ---
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var responseData = await TestUtilities.DeserializeResponseAsync<List<ProfileHeaderResponse>>(response);
        responseData.Should().NotBeNull();
    }

    [Fact(DisplayName = "Should return 404 Not Found when the profile ID does not exist")]
    public async Task GetProfileHeaderNotFound_WhenProfileDoesNotExist()
    {
        // --- Given ---
        var nonExistentProfileId = Guid.NewGuid();
        await _testFixture.ResetDatabaseAsync();

        // --- When ---
        var response = await _httpClient.GetAsync($"{RequestUriController}/{nonExistentProfileId}/ProfileHeader");

        // --- Then ---
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact(DisplayName = "Should return 200 OK with an empty list when the profile exists but has no ProfileHeader")]
    public async Task GetOkAndEmptyList_WhenProfileExistsButHasNoProfileHeader()
    {
        // --- Given ---

        var profileWithoutProfileHeader = await SetupProfileAsync();

        // --- When ---
        var response = await _httpClient.GetAsync($"{RequestUriController}/{profileWithoutProfileHeader.Id.Value}/ProfileHeader");

        // --- Then ---
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var responseData = await TestUtilities.DeserializeResponseAsync<List<ProfileHeaderResponse>>(response);
        responseData.Should().NotBeNull().And.BeEmpty();
    }

    [Fact(DisplayName = "Should return 204 No Content when updating an existing ProfileHeader")]
    public async Task UpdateProfileHeader_WhenProfileAndProfileHeaderExistAndPayloadIsValid()
    {
        // --- Given ---
        var profile = await SetupProfileAsync();
        var profileHeaderToUpdate = profile.GetProfileHeader().First();
        var newProficiencyLevel = (ushort) (profileHeaderToUpdate.ProficiencyLevel + 1 >= ProfileHeader.MaximumProficiencyLevel
            ? profileHeaderToUpdate.ProficiencyLevel - 1
            : profileHeaderToUpdate.ProficiencyLevel + 1);
        var ProfileHeaderToUpdate = new List<ProfileHeaderBase>
        {
            new() { Id = profileHeaderToUpdate.Id.Value, ProficiencyLevel = newProficiencyLevel }
        };
        var profileHeaderData = _fixture.Build<ProfileHeaderBase>()
            .With(s => s.ProfileHeader, ProfileHeaderToUpdate)
            .Create();
        var command = _fixture.Build<ProfileHeaderCommand>()
            .With(c => c.ProfileHeaderData, profileHeaderData)
            .Create();

        var requestUri = $"{RequestUriController}/{profile.Id.Value}/ProfileHeader/{profileHeaderToUpdate.Id.Value}";

        // --- When ---
        var response = await _httpClient.PutAsJsonAsync(requestUri, command);

        // --- Then ---
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        var entityUpdated = await FindProfileHeaderByIdAsync(profile.Id);

        entityUpdated.Should().NotBeNull();
        entityUpdated.ProfileId.Should().Be(profile.Id);
        var commandBase = command.ProfileHeaderData;
        entityUpdated.ProfileHeader.Should().HaveSameCount(profile.GetProfileHeader());
        var profileHeaderUpdated = entityUpdated.ProfileHeader.First(x => x.Id == profileHeaderToUpdate.Id);
        profileHeaderUpdated.ProficiencyLevel.Should().Be(newProficiencyLevel);
    }

    [Fact(DisplayName = "Should return 404 Not Found when the profile ID does not exist for an update")]
    public async Task UpdateProfileHeader_WhenProfileDoesNotExist()
    {
        // --- Given ---
        var nonExistentProfileId = Guid.NewGuid();
        var command = _fixture.Create<ProfileHeaderCommand>();
        var requestUri = $"{RequestUriController}/{nonExistentProfileId}/ProfileHeader/{nonExistentProfileId}";

        // --- When ---
        var response = await _httpClient.PutAsJsonAsync(requestUri, command);

        // --- Then ---
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact(DisplayName = "Should return 404 Not Found when the ProfileHeader does not belong to the profile")]
    public async Task UpdateProfileHeader_WhenProfileHeaderDoesNotExistInProfile()
    {
        // --- Given ---
        var profile = await SetupProfileAsync();
        var profileHeaderData = GetProfileHeaderData(profile.Id.Value);
        var nonExistentProfileHeaderId = Guid.NewGuid();
        var command = _fixture.Build<ProfileHeaderCommand>()
            .With(x => x.ProfileHeaderData, profileHeaderData)
            .Create();
        var requestUri = $"{RequestUriController}/{profile.Id.Value}/ProfileHeader/{nonExistentProfileHeaderId}";

        // --- When ---
        var response = await _httpClient.PutAsJsonAsync(requestUri, command);

        // --- Then ---
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact(DisplayName = "Should return 204 No Content when delete an existing ProfileHeader to ProfileHeader")]
    public async Task DeleteProfileHeader_WhenProfileAndProfileHeaderExist()
    {
        // --- Given ---
        var profile = await SetupProfileAsync();
        var profileHeaderToDelete = profile.GetProfileHeader().FirstOrDefault();
        var requestUri = $"{RequestUriController}/{profile.Id.Value}/ProfileHeader/{profileHeaderToDelete!.Id.Value}";

        // --- When ---
        var response = await _httpClient.DeleteAsync(requestUri);

        // --- Then ---
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        await using var scope = _testFixture.GetProvider().CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<JobMagnetDbContext>();
        var ProfileHeader = await dbContext.ProfileHeader.Include(x => x.ProfileHeader)
            .FirstOrDefaultAsync(x => x.Id == profile.ProfileHeader.Id);

        ProfileHeader.Should().NotBeNull();
        ProfileHeader.ProfileHeader.Should().HaveCount(_ProfileHeaderCount - 1);
        ProfileHeader.ProfileHeader.Should().NotContain(p => p.Id == profileHeaderToDelete.Id);
    }

    [Fact(DisplayName = "Should return 404 Not Found when the profile ID does not exist for a delete")]
    public async Task DeleteProfileHeader_WhenProfileDoesNotExist()
    {
        // --- Given ---
        var nonExistentProfileId = Guid.NewGuid();
        var requestUri = $"{RequestUriController}/{nonExistentProfileId}/ProfileHeader/{Guid.NewGuid()}";

        // --- When ---
        var response = await _httpClient.DeleteAsync(requestUri);

        // --- Then ---
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact(DisplayName = "Should return 404 Not Found when deleting a ProfileHeader that does not belong to the profile")]
    public async Task DeleteProfileHeader_WhenProfileHeaderDoesNotExistInProfile()
    {
        // --- Given ---
        var profile = await SetupProfileAsync();
        var nonExistentProfileHeaderId = Guid.NewGuid();
        var requestUri = $"{RequestUriController}/{profile.Id.Value}/ProfileHeader/{nonExistentProfileHeaderId}";

        // --- When ---
        var response = await _httpClient.DeleteAsync(requestUri);

        // --- Then ---
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact(DisplayName = "Should return 204 No Content when arranging ProfileHeader with a valid order")]
    public async Task ArrangeProfileHeader_WhenProfileExistsAndPayloadIsValid_ShouldUpdatePositionsInDb()
    {
        // --- Given ---
        var profile = await SetupProfileAsync();
        var ProfileHeader = profile.GetProfileHeader();
        var newOrderExpected = ProfileHeader.OrderByDescending(x => x.Position).Select(x => x.Id).ToList();
        var newOrderCommand = newOrderExpected.Select(x => x.Value);

        var requestUri = $"{RequestUriController}/{profile.Id.Value}/ProfileHeader/arrange";

        // --- When ---
        var response = await _httpClient.PutAsJsonAsync(requestUri, newOrderCommand);

        // --- Then ---
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        var ProfileHeaderUpdated = await FindProfileHeaderByIdAsync(profile.Id);
        var ProfileHeaderOrderedByPosition = ProfileHeaderUpdated!.ProfileHeader.OrderBy(p => p.Position).Select(x => x.Id);
        ProfileHeaderOrderedByPosition.Should().BeEquivalentTo(newOrderExpected);
    }
*/

    private async Task<ProfileHeader?> FindProfileHeaderByIdAsync(ProfileId id)
    {
        await using var scope = _testFixture.GetProvider().CreateAsyncScope();
        var queryRepository = scope.ServiceProvider.GetRequiredService<IProfileQueryRepository>();
        var entityCreated = await queryRepository
            .WithProfileHeader()
            .WhereCondition(x => x.Id == id)
            .BuildFirstOrDefaultAsync(CancellationToken.None, true);

        return entityCreated!.Header;
    }
}