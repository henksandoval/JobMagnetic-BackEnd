using JobMagnet.Application.Contracts.Commands.Talent;
using JobMagnet.Application.Contracts.Responses.TalentShowcase;
using JobMagnet.Application.Mappers;
using JobMagnet.Domain.Aggregates.Profiles;
using JobMagnet.Domain.Aggregates.Profiles.ValueObjects;
using Microsoft.AspNetCore.Mvc;

namespace JobMagnet.Host.Controllers.V1;

public partial class ProfileController
{
    [HttpPost("{profileId:guid}/talent")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IResult> AddTalentsToProfileAsync(Guid profileId, [FromBody] TalentCommand command, CancellationToken cancellationToken)
    {
        if (profileId != command.TalentData?.ProfileId)
            throw new ArgumentException($"{nameof(command.TalentData.ProfileId)} does not match the profileId in the route.");

        var profile = await GetProfileWithTalent(profileId, cancellationToken).ConfigureAwait(false);

        if (profile is null)
            return Results.NotFound();
        
        var talent = profile.TalentShowcase.AddTalent(
            command.TalentData.Description ?? string.Empty
        );

        profileCommandRepository.Update(profile);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        var result = talent.ToModel();
        
        return Results.CreatedAtRoute("GetTalentsByProfile", new { profileId }, result);
    }

    [HttpGet("{profileId:guid}/talents", Name = "GetTalentsByProfile")]
    [ProducesResponseType(typeof(IEnumerable<TalentResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IResult> GetTalentsByProfileAsync(Guid profileId, CancellationToken cancellationToken)
    {
        var profile = await GetProfileWithTalent(profileId, cancellationToken).ConfigureAwait(false);

        if (profile is null)
            return Results.NotFound();

        var response = profile.Talents
            .Select(talent => talent.ToModel())
            .ToList();

        return Results.Ok(response);
    }

    private async Task<Profile?> GetProfileWithTalent(Guid profileId, CancellationToken cancellationToken)
    {
        return await queryRepository
            .WhereCondition(p => p.Id == new ProfileId(profileId))
            .WithTalents()
            .BuildFirstOrDefaultAsync(cancellationToken)
            .ConfigureAwait(false);
    }
}