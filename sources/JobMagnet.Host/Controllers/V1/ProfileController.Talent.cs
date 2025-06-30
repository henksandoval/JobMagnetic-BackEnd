using JobMagnet.Application.Contracts.Commands.Talent;
using JobMagnet.Application.Mappers;
using JobMagnet.Domain.Aggregates.Profiles;
using JobMagnet.Domain.Aggregates.Profiles.Entities;
using Microsoft.AspNetCore.Mvc;

namespace JobMagnet.Host.Controllers.V1;

public partial class ProfileController
{
    [HttpPost("{profileId:guid}/talents")]
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
        
        await talentCommandRepository.CreateAsync(talent, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        var result = talent.ToModel();
        
        return Results.CreatedAtRoute("GettalentById", new { profileId = profile.Id }, result);

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