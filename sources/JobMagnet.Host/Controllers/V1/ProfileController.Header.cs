using CommunityToolkit.Diagnostics;
using JobMagnet.Application.Contracts.Commands.ProfileHeader;
using JobMagnet.Application.Contracts.Responses.ProfileHeader;
using JobMagnet.Application.Mappers;
using JobMagnet.Domain.Aggregates.Profiles;
using JobMagnet.Domain.Aggregates.Profiles.Entities;
using JobMagnet.Domain.Aggregates.Profiles.ValueObjects;
using Microsoft.AspNetCore.Mvc;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace JobMagnet.Host.Controllers.V1;

public partial class ProfileController
{
    [HttpPost("{profileId:guid}/header")]
    [ProducesResponseType(typeof(ProfileHeaderResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IResult> AddProfileHeaderToProfileAsync(Guid profileId, [FromBody] ProfileHeaderCommand command, CancellationToken cancellationToken)
    {
        if (profileId != command.ProfileHeaderData?.ProfileId)
            throw new ArgumentException($"{nameof(command.ProfileHeaderData.ProfileId)} must be equal to profileId.");

        var profile = await GetProfileWithProfileHeader(profileId, cancellationToken).ConfigureAwait(false);

        if (profile is null)
            return Results.NotFound();

        var data = command.ProfileHeaderData;

        if (profile.HaveHeader)
            throw new InvalidOperationException($"The profile ({profileId}) already has a header, please use the PUT method to update it.");

        profile.AddHeader(
            guidGenerator,
            data.Title ?? string.Empty,
            data.Suffix ?? string.Empty,
            data.JobTitle ?? string.Empty,
            data.About ?? string.Empty,
            data.Summary ?? string.Empty,
            data.Overview ?? string.Empty,
            data.Address ?? string.Empty);

        profileCommandRepository.Update(profile);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        var result = profile.Header!.ToModel();

        return Results.CreatedAtRoute("GetHeaderByProfile", new { profileId }, result);
    }

    [HttpGet("{profileId:guid}/header", Name = "GetHeaderByProfile")]
    [ProducesResponseType(typeof(ProfileHeaderResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IResult> GetHeaderByProfileAsync(Guid profileId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    private async Task<Profile?> GetProfileWithProfileHeader(Guid profileId, CancellationToken cancellationToken)
    {
        return await queryRepository
            .WhereCondition(p => p.Id == new ProfileId(profileId))
            .WithProfileHeader()
            .BuildFirstOrDefaultAsync(cancellationToken)
            .ConfigureAwait(false);
    }
}