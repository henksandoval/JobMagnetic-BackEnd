using JobMagnet.Application.Contracts.Commands.Skill;
using JobMagnet.Application.Contracts.Responses.Skill;
using JobMagnet.Application.Mappers;
using JobMagnet.Domain.Aggregates.Profiles;
using JobMagnet.Domain.Aggregates.Profiles.Entities;
using JobMagnet.Domain.Aggregates.Skills;
using JobMagnet.Domain.Aggregates.Skills.Entities;
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

        if (profile.SkillSet is null)
        {
            var skillSet = SkillSet.CreateInstance(
                guidGenerator,
                clock,
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

                profile.SkillSet!.AddSkill(
                    guidGenerator,
                    clock,
                    skill.ProficiencyLevel,
                    skillTypeToUse
                );
            }
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        var result = profile.SkillSet!.ToModel();

        return Results.CreatedAtRoute("GetSkillsByProfile", new { profileId }, result);
    }

    [HttpGet("{profileId:guid}/Skills", Name = "GetSkillsByProfile")]
    [ProducesResponseType(typeof(IEnumerable<SkillResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IResult> GetSkillsByProfileAsync(Guid profileId, CancellationToken cancellationToken)
    {
        var profile = await GetProfileWithSkills(profileId, cancellationToken);

        if (profile is null)
            return Results.NotFound();

        var response = profile.SkillSet!.Skills
            .Select(skills => skills.ToModel())
            .ToList();

        return Results.Ok(response);
    }
/*
    [HttpPut("{profileId:guid}/Skills/{SkillSetId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IResult> UpdateskillsAsync(Guid profileId, Guid SkillSetId, [FromBody] SkillCommand command,
        CancellationToken cancellationToken)
    {
        var profile = await GetProfileWithSkills(profileId, cancellationToken).ConfigureAwait(false);

        if (profile is null)
            return Results.NotFound();

        var data = command.SkillSetData;

        Guard.IsNotNull(data);

        try
        {
            var updatedskills = profile.SkillSet.Updateskills(
                new SkillSetId(SkillSetId),
                data.Title ?? string.Empty,
                data.Description ?? string.Empty,
                data.UrlLink ?? string.Empty,
                data.UrlImage ?? string.Empty,
                data.UrlVideo ?? string.Empty,
                data.Type ?? string.Empty
            );

            skillCommandRepository.Update(updatedskills);
        }
        catch (NotFoundException ex)
        {
            return Results.NotFound(new { ex.Message });
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Results.NoContent();
    }
*/
    [HttpDelete("{profileId:guid}/Skills/{skillId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IResult> DeleteSkillsAsync(Guid profileId, Guid skillId, CancellationToken cancellationToken)
    {
        var profile = await GetProfileWithSkills(profileId, cancellationToken).ConfigureAwait(false);

        if (profile is null)
            return Results.NotFound();

        if (!profile.HaveSkillSet)
            throw new ApplicationException($"The profile {profileId} does not have skills set.");

        try
        {
            profile.SkillSet!.RemoveSkill(new SkillId(skillId));
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