using CommunityToolkit.Diagnostics;
using JobMagnet.Application.Contracts.Commands.ProfileHeader;
using JobMagnet.Application.Contracts.Responses.ProfileHeader;
using JobMagnet.Application.Mappers;
using JobMagnet.Domain.Aggregates.Profiles;
using JobMagnet.Domain.Aggregates.Profiles.Entities;
using JobMagnet.Domain.Aggregates.Profiles.ValueObjects;
using JobMagnet.Domain.Exceptions;
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

        var profile = await GetProfileWithHeader(profileId, cancellationToken).ConfigureAwait(false);

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
        var profile = await GetProfileWithHeader(profileId, cancellationToken);

        if (profile?.Header is null)
            return Results.NotFound();

        var response = profile.Header!.ToModel();

        return Results.Ok(response);
    }

    [HttpPut("{profileId:guid}/header")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IResult> UpdateProfileHeaderAsync(Guid profileId, [FromBody] ProfileHeaderCommand command,
        CancellationToken cancellationToken)
    {
        if (profileId != command.ProfileHeaderData?.ProfileId)
            throw new ArgumentException($"{nameof(command.ProfileHeaderData.ProfileId)} must be equal to profileId.");

        var profile = await GetProfileWithHeader(profileId, cancellationToken).ConfigureAwait(false);

        if (profile?.Header is null)
            return Results.NotFound();

        var data = command.ProfileHeaderData;

        Guard.IsNotNull(data);

        profile.UpdateHeader(
            data.Title ?? string.Empty,
            data.Suffix ?? string.Empty,
            data.JobTitle ?? string.Empty,
            data.About ?? string.Empty,
            data.Summary ?? string.Empty,
            data.Overview ?? string.Empty,
            data.Address ?? string.Empty
        );

        profileCommandRepository.Update(profile);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Results.NoContent();
    }

    [HttpDelete("{profileId:guid}/header/{skillId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IResult> DeleteHeaderAsync(Guid profileId, Guid headerId, CancellationToken cancellationToken)
    {
        var profile = await GetProfileWithHeader(profileId, cancellationToken).ConfigureAwait(false);

        if (profile is null)
            return Results.NotFound();

        if (!profile.HaveHeader)
            throw new ApplicationException($"The profile {profileId} does not have a header.");

        try
        {
            profile.RemoveHeader(clock);
            profileCommandRepository.Update(profile);
        }
        catch (NotFoundException ex)
        {
            return Results.NotFound(new { ex.Message });
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Results.NoContent();
    }

    private async Task<Profile?> GetProfileWithHeader(Guid profileId, CancellationToken cancellationToken)
    {
        return await queryRepository
            .WhereCondition(p => p.Id == new ProfileId(profileId))
            .WithProfileHeader()
            .BuildFirstOrDefaultAsync(cancellationToken)
            .ConfigureAwait(false);
    }
}