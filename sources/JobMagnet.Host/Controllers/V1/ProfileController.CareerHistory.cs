using CommunityToolkit.Diagnostics;
using JobMagnet.Application.Contracts.Commands.CareerHistory;
using JobMagnet.Application.Contracts.Responses.Base;
using JobMagnet.Application.Contracts.Responses.CareerHistory;
using JobMagnet.Application.Mappers;
using JobMagnet.Domain.Aggregates.Profiles;
using JobMagnet.Domain.Aggregates.Profiles.ValueObjects;
using JobMagnet.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace JobMagnet.Host.Controllers.V1;

public partial class ProfileController
{
    [HttpPost("{profileId:guid}/career-history")]
    [ProducesResponseType(typeof(CareerHistoryResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IResult> CreateCareerHistoryAsync(Guid profileId, [FromBody] CareerHistoryCommand command, CancellationToken cancellationToken)
    {
        var profile = await GetProfileWithCareerHistory(profileId, cancellationToken).ConfigureAwait(false);

        if (profile is null)
            return Results.NotFound();

        if (profile.HaveCareerHistory)
            return Results.BadRequest($"The profile {profileId} have a career history already.");

        var data = command.CareerHistoryData;
        Guard.IsNotNull(data);

        profile.CreateCareerHistory(
            guidGenerator,
            data.Introduction ?? string.Empty
        );

        foreach (var education in data.Education)
            profile.AddAcademicDegreeToCareerHistory(
                guidGenerator,
                education.Degree ?? string.Empty,
                education.InstitutionName ?? string.Empty,
                education.InstitutionLocation ?? string.Empty,
                education.StartDate,
                education.EndDate,
                education.Description ?? string.Empty);

        foreach (var workExperience in data.WorkExperiences)
            profile.AddWorkExperienceToCareerHistory(
                guidGenerator,
                workExperience.JobTitle ?? string.Empty,
                workExperience.CompanyName ?? string.Empty,
                workExperience.CompanyLocation ?? string.Empty,
                workExperience.StartDate,
                workExperience.EndDate,
                workExperience.Description ?? string.Empty);

        profileCommandRepository.Update(profile);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        var result = profile.CareerHistory!.ToModel();

        return Results.CreatedAtRoute("GetCareerHistory", new { profileId }, result);
    }

    [HttpGet("{profileId:guid}/career-history", Name = "GetCareerHistory")]
    [ProducesResponseType(typeof(CareerHistoryResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IResult> GetCareerHistoryAsync(Guid profileId, CancellationToken cancellationToken)
    {
        var profile = await GetProfileWithCareerHistory(profileId, cancellationToken);

        if (profile is null)
            return Results.NotFound();

        if (profile.CareerHistory is null)
            return Results.NotFound();

        var response = profile.CareerHistory.ToModel();

        return Results.Ok(response);
    }

    [HttpPut("{profileId:guid}/career-history")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IResult> UpdateCareerHistoryIntroductionAsync(Guid profileId, [FromBody] CareerHistoryCommand command,
        CancellationToken cancellationToken)
    {
        var profile = await GetProfileWithCareerHistory(profileId, cancellationToken).ConfigureAwait(false);

        if (profile is null)
            return Results.NotFound();

        var data = command.CareerHistoryData;
        Guard.IsNotNull(data);

        try
        {
            profile.UpdateCareerHistoryIntroduction(data.Introduction ?? string.Empty);
            profileCommandRepository.Update(profile);
        }
        catch (NotFoundException ex)
        {
            return Results.NotFound(new { ex.Message });
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Results.NoContent();
    }

    [HttpDelete("{profileId:guid}/career-history")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IResult> DeleteCareerHistoryAsync(Guid profileId, CancellationToken cancellationToken)
    {
        var profile = await GetProfileWithCareerHistory(profileId, cancellationToken).ConfigureAwait(false);

        if (profile is null)
            return Results.NotFound();

        try
        {
            profile.RemoveCareerHistory();
            profileCommandRepository.Update(profile);
        }
        catch (NotFoundException ex)
        {
            return Results.NotFound(new { ex.Message });
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Results.NoContent();
    }

    [HttpPost("{profileId:guid}/career-history/academic-degree")]
    [ProducesResponseType(typeof(AcademicDegreeBase), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IResult> AddQualificationAsync(Guid profileId, [FromBody] AcademicDegreeCommand command, CancellationToken cancellationToken)
    {
        var profile = await GetProfileWithCareerHistory(profileId, cancellationToken).ConfigureAwait(false);

        if (profile is null)
            return Results.NotFound();

        try
        {
            var data = command.AcademicDegreeData;
            Guard.IsNotNull(data);

            var qualification = profile.AddAcademicDegreeToCareerHistory(
                guidGenerator,
                data.Degree ?? string.Empty,
                data.InstitutionName ?? string.Empty,
                data.InstitutionLocation ?? string.Empty,
                data.StartDate,
                data.EndDate,
                data.Description ?? string.Empty
            );

            profileCommandRepository.Update(profile);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            var result = new AcademicDegreeBase(
                qualification.Degree,
                qualification.InstitutionName,
                qualification.InstitutionLocation,
                qualification.Description,
                qualification.StartDate,
                qualification.EndDate,
                qualification.Id.Value
            );

            return Results.CreatedAtRoute("GetCareerHistory", new { profileId }, result);
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

    [HttpPut("{profileId:guid}/career-history/academic-degree/{qualificationId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IResult> UpdateAcademicDegreeAsync(Guid profileId, Guid qualificationId, [FromBody] AcademicDegreeCommand command,
        CancellationToken cancellationToken)
    {
        var profile = await GetProfileWithCareerHistory(profileId, cancellationToken).ConfigureAwait(false);

        if (profile is null)
            return Results.NotFound();

        var data = command.AcademicDegreeData;
        Guard.IsNotNull(data);

        try
        {
            profile.UpdateAcademicDegreeInCareerHistory(
                new AcademicDegreeId(qualificationId),
                data.Degree ?? string.Empty,
                data.InstitutionName ?? string.Empty,
                data.InstitutionLocation ?? string.Empty,
                data.StartDate,
                data.EndDate,
                data.Description ?? string.Empty
            );

            profileCommandRepository.Update(profile);
        }
        catch (NotFoundException ex)
        {
            return Results.NotFound(new { ex.Message });
        }
        catch (BusinessRuleValidationException ex)
        {
            return Results.BadRequest(new { ex.Message });
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Results.NoContent();
    }

    [HttpDelete("{profileId:guid}/career-history/academic-degree/{academicDegreeId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IResult> DeleteQualificationAsync(Guid profileId, Guid academicDegreeId, CancellationToken cancellationToken)
    {
        var profile = await GetProfileWithCareerHistory(profileId, cancellationToken).ConfigureAwait(false);

        if (profile is null)
            return Results.NotFound();

        try
        {
            profile.RemoveAcademicDegreeFromCareerHistory(new AcademicDegreeId(academicDegreeId));
            profileCommandRepository.Update(profile);
        }
        catch (NotFoundException ex)
        {
            return Results.NotFound(new { ex.Message });
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Results.NoContent();
    }

    [HttpPost("{profileId:guid}/career-history/work-experiences")]
    [ProducesResponseType(typeof(WorkExperienceBase), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IResult> AddWorkExperienceAsync(Guid profileId, [FromBody] WorkExperienceCommand command, CancellationToken cancellationToken)
    {
        var profile = await GetProfileWithCareerHistory(profileId, cancellationToken).ConfigureAwait(false);

        if (profile is null)
            return Results.NotFound();

        try
        {
            var data = command.WorkExperienceData;
            Guard.IsNotNull(data);

            var workExperience = profile.AddWorkExperienceToCareerHistory(
                guidGenerator,
                data.JobTitle ?? string.Empty,
                data.CompanyName ?? string.Empty,
                data.CompanyLocation ?? string.Empty,
                data.StartDate,
                data.EndDate,
                data.Description ?? string.Empty
            );

            profileCommandRepository.Update(profile);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            var result = new WorkExperienceBase(
                workExperience.JobTitle,
                workExperience.CompanyName,
                workExperience.CompanyLocation,
                workExperience.StartDate,
                workExperience.EndDate,
                workExperience.Description,
                workExperience.Highlights.Select(h => h.Description).ToList(),
                workExperience.Id.Value
            );

            return Results.CreatedAtRoute("GetCareerHistory", new { profileId }, result);
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

    [HttpPut("{profileId:guid}/career-history/work-experiences/{workExperienceId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IResult> UpdateWorkExperienceAsync(Guid profileId, Guid workExperienceId, [FromBody] WorkExperienceCommand command,
        CancellationToken cancellationToken)
    {
        var profile = await GetProfileWithCareerHistory(profileId, cancellationToken).ConfigureAwait(false);

        if (profile is null)
            return Results.NotFound();

        var data = command.WorkExperienceData;
        Guard.IsNotNull(data);

        try
        {
            profile.UpdateWorkExperienceInCareerHistory(
                new WorkExperienceId(workExperienceId),
                data.JobTitle ?? string.Empty,
                data.CompanyName ?? string.Empty,
                data.CompanyLocation ?? string.Empty,
                data.StartDate,
                data.EndDate,
                data.Description ?? string.Empty
            );

            profileCommandRepository.Update(profile);
        }
        catch (NotFoundException ex)
        {
            return Results.NotFound(new { ex.Message });
        }
        catch (BusinessRuleValidationException ex)
        {
            return Results.BadRequest(new { ex.Message });
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Results.NoContent();
    }

    [HttpDelete("{profileId:guid}/career-history/work-experiences/{workExperienceId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IResult> DeleteWorkExperienceAsync(Guid profileId, Guid workExperienceId, CancellationToken cancellationToken)
    {
        var profile = await GetProfileWithCareerHistory(profileId, cancellationToken).ConfigureAwait(false);

        if (profile is null)
            return Results.NotFound();

        try
        {
            profile.RemoveWorkExperienceFromCareerHistory(new WorkExperienceId(workExperienceId));
            profileCommandRepository.Update(profile);
        }
        catch (NotFoundException ex)
        {
            return Results.NotFound(new { ex.Message });
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Results.NoContent();
    }

    private async Task<Profile?> GetProfileWithCareerHistory(Guid profileId, CancellationToken cancellationToken)
    {
        return await queryRepository
            .WhereCondition(p => p.Id == new ProfileId(profileId))
            .WithCareerHistory()
            .BuildFirstOrDefaultAsync(cancellationToken)
            .ConfigureAwait(false);
    }
}