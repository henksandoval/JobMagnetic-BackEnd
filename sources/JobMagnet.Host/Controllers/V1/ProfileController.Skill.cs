using CommunityToolkit.Diagnostics;
using JobMagnet.Application.Contracts.Commands.Skill;
using JobMagnet.Application.Contracts.Responses.Skill;
using JobMagnet.Application.Mappers;
using JobMagnet.Domain.Aggregates.Profiles;
using JobMagnet.Domain.Aggregates.Profiles.Entities;
using JobMagnet.Domain.Aggregates.Profiles.ValueObjects;
using JobMagnet.Domain.Aggregates.SkillTypes;
using JobMagnet.Domain.Aggregates.SkillTypes.Entities;
using JobMagnet.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace JobMagnet.Host.Controllers.V1;

public partial class ProfileController
{
    [HttpPost("{profileId:guid}/skills")]
    [ProducesResponseType(typeof(SkillResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IResult> AddSkillToProfileAsync(Guid profileId, [FromBody] SkillCommand command, CancellationToken cancellationToken)
    {
        if (profileId != command.SkillSetData?.ProfileId)
            throw new ArgumentException($"{nameof(command.SkillSetData.ProfileId)} must be equal to profileId.");

        var profile = await GetProfileWithSkills(profileId, cancellationToken).ConfigureAwait(false);

        if (profile is null)
            return Results.NotFound();

        var data = command.SkillSetData;

        if (!profile.HaveSkillSet)
        {
            var skillSet = SkillSet.CreateInstance(
                guidGenerator,
                profile.Id,
                data.Overview ?? string.Empty);
            profile.AddSkillSet(skillSet);
        }

        if (data.Skills.Any())
        {
            var defaultCategoryLazy = new Lazy<Task<SkillCategory>>(async () =>
                await skillCategoryRepository
                    .GetByIdAsync(new SkillCategoryId(SkillCategory.DefaultCategoryId), cancellationToken)
                    .ConfigureAwait(false) ?? throw new InvalidOperationException("The DefaultCategory is missing."));

            var nameSkills = data.Skills.DistinctBy(x => x.Name).Select(x => x.Name);
            var resolvedTypes = await skillTypeResolverService.ResolveAsync(nameSkills!, cancellationToken)
                .ConfigureAwait(false);

            foreach (var skill in data.Skills.Where(skill => !string.IsNullOrWhiteSpace(skill.Name)))
            {
                resolvedTypes.TryGetValue(skill.Name!, out var maybeSkillType);

                SkillType skillTypeToUse;

                if (maybeSkillType.HasValue)
                    skillTypeToUse = maybeSkillType.Value;
                else
                {
                    var defaultCategory = await defaultCategoryLazy.Value;

                    skillTypeToUse = SkillType.CreateInstance(
                        guidGenerator,
                        clock,
                        skill.Name ?? string.Empty,
                        defaultCategory
                    );
                }

                profile.AddSkill(
                    guidGenerator,
                    skill.ProficiencyLevel,
                    skillTypeToUse
                );
            }
        }

        profileCommandRepository.Update(profile);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        var result = profile.SkillSet!.ToModel();

        return Results.CreatedAtRoute("GetSkillsByProfile", new { profileId }, result);
    }

    [HttpGet("{profileId:guid}/skills", Name = "GetSkillsByProfile")]
    [ProducesResponseType(typeof(IEnumerable<SkillResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IResult> GetSkillsByProfileAsync(Guid profileId, CancellationToken cancellationToken)
    {
        var profile = await GetProfileWithSkills(profileId, cancellationToken);

        if (profile is null)
            return Results.NotFound();

        if (!profile.HaveSkillSet)
            return Results.NoContent();

        var response = profile.GetSkills()
            .Select(skills => skills.ToModel())
            .ToList();

        return Results.Ok(response);
    }

    [HttpPut("{profileId:guid}/skills/{skillSetId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IResult> UpdateSkillsAsync(Guid profileId, Guid skillSetId, [FromBody] SkillCommand command,
        CancellationToken cancellationToken)
    {
        var profile = await GetProfileWithSkills(profileId, cancellationToken).ConfigureAwait(false);

        if (profile is null || !profile.HaveSkillSet || profile.SkillSet!.Id != new SkillSetId(skillSetId))
            return Results.NotFound();

        var data = command.SkillSetData;

        Guard.IsNotNull(data);

        try
        {
            profile.UpdateSkillSet(data.Overview ?? string.Empty);
            foreach (var skill in data.Skills)
                profile.UpdateSkill(new SkillId(skill.Id), skill.ProficiencyLevel);

            profileCommandRepository.Update(profile);
        }
        catch (NotFoundException ex)
        {
            return Results.NotFound(new { ex.Message });
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Results.NoContent();
    }

    [HttpPut("{profileId:guid}/skills/arrange")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IResult> ArrangeSkillsAsync(Guid profileId, [FromBody] List<Guid> orderedSkillsIds, CancellationToken cancellationToken)
    {
        try
        {
            var profile = await GetProfileWithSkills(profileId, cancellationToken);

            if (profile is null)
                return Results.NotFound();

            var typedIds = orderedSkillsIds.Select(id => new SkillId(id));

            profile.ArrangeSkills(typedIds);

            profileCommandRepository.Update(profile);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Results.NoContent();
        }
        catch (NotFoundException ex)
        {
            return Results.NotFound(new { ex.Message });
        }
        catch (BusinessRuleValidationException ex)
        {
            return Results.BadRequest(new { ex.Message });
        }
    }

    [HttpDelete("{profileId:guid}/skills/{skillId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IResult> DeleteSkillsAsync(Guid profileId, Guid headerId, CancellationToken cancellationToken)
    {
        var profile = await GetProfileWithSkills(profileId, cancellationToken).ConfigureAwait(false);

        if (profile is null)
            return Results.NotFound();

        if (!profile.HaveSkillSet)
            throw new ApplicationException($"The profile {profileId} does not have skills set.");

        try
        {
            profile.RemoveSkill(new SkillId(headerId));
            profileCommandRepository.Update(profile);
        }
        catch (NotFoundException ex)
        {
            return Results.NotFound(new { ex.Message });
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Results.NoContent();
    }

    private async Task<Profile?> GetProfileWithSkills(Guid profileId, CancellationToken cancellationToken)
    {
        return await queryRepository
            .WhereCondition(p => p.Id == new ProfileId(profileId))
            .WithSkills()
            .BuildFirstOrDefaultAsync(cancellationToken)
            .ConfigureAwait(false);
    }
}