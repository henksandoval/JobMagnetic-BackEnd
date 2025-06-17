using JobMagnet.Application.UseCases.CvParser.DTO.ParsingDTOs;
using JobMagnet.Domain.Core.Entities;
using JobMagnet.Domain.Ports.Repositories.Base;

namespace JobMagnet.Application.Services;

public interface IProfileFactoryService
{
    Task<ProfileEntity> CreateProfileFromDtoAsync(ProfileParseDto profileDto, CancellationToken cancellationToken);
}

public class ProfileFactoryService(
    IQueryRepository<SkillType, int> skillTypeRepository,
    IQueryRepository<ContactTypeEntity, int> contactTypeRepository) : IProfileFactoryService
{
    public async Task<ProfileEntity> CreateProfileFromDtoAsync(ProfileParseDto profileDto, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(profileDto);

        var profileEntity = new ProfileEntity
        {
            Id = 0,
            FirstName = profileDto.FirstName,
            SecondLastName = profileDto.SecondLastName,
            LastName = profileDto.LastName,
            MiddleName = profileDto.MiddleName,
            BirthDate = profileDto.BirthDate,
            ProfileImageUrl = profileDto.ProfileImageUrl
        };

        profileEntity.AddTalents(BuildTalents(profileDto.Talents));
        profileEntity.AddTestimonials(BuildTestimonials(profileDto.Testimonials));
        profileEntity.AddPortfolioItems(BuildPortfolio(profileDto.PortfolioGallery));
/*
        if (profileDto.Resume is not null)
        {
            profileEntity.AddResume(await BuildResumeAsync(profileDto.Resume, cancellationToken));
        }

        if (profileDto.SkillSet is not null)
        {
            profileEntity.AddSkill(await BuildSkillSetAsync(profileDto.SkillSet, cancellationToken));
        }
*/
        return profileEntity;
    }
/*
    private async Task<ResumeEntity> BuildResumeAsync(ResumeParseDto resumeDto, CancellationToken cancellationToken)
    {
        var resumeEntity = new ResumeEntity(resumeDto.About ?? string.Empty, resumeDto.Overview ?? string.Empty);

        resumeEntity.AddSummary(BuildSummary(resumeDto.Summary));
        resumeEntity.AddEducationHistory(BuildEducationHistory(resumeDto.Education));
        resumeEntity.AddWorkExperience(BuildWorkExperience(resumeDto.WorkExperience));
        resumeEntity.AddContactInfo(await BuildContactInfoAsync(resumeDto.ContactInfo, cancellationToken));

        return resumeEntity;
    }
*/
    private List<TalentEntity> BuildTalents(List<TalentParseDto>? talentDtos)
    {
        if (talentDtos is null) return [];

        return talentDtos.Select(dto => new TalentEntity
        {
            Id = 0,
            Description = dto.Description!
        }).ToList();
    }

    private List<TestimonialEntity> BuildTestimonials(List<TestimonialParseDto>? testimonialDtos)
    {
        if (testimonialDtos is null) return [];

        return testimonialDtos.Select(dto => new TestimonialEntity
        {
            Id = 0,
            Name = dto.Name!,
            Feedback = dto.Feedback!,
            JobTitle = dto.JobTitle!,
            PhotoUrl = dto.PhotoUrl!
        }).ToList();
    }

    private List<PortfolioGalleryEntity> BuildPortfolio(List<PortfolioGalleryParseDto>? portfolioDtos)
    {
        if (portfolioDtos is null) return [];

        return portfolioDtos.Select(dto => new PortfolioGalleryEntity
        {
            Id = 0,
            Description = dto.Description!,
            Title = dto.Title!,
            Type = dto.Type!,
            UrlImage = dto.UrlImage!
        }).ToList();
    }
/*
    private async Task<List<ContactInfoEntity>> BuildContactInfoAsync(List<ContactInfoParseDto>? contactDtos, CancellationToken cancellationToken)
    {
        if (contactDtos is null || contactDtos.Count == 0) return [];

        var contactTypeNames = contactDtos.Select(c => c.ContactType!).Distinct().ToList();

        var existingContactTypes = await contactTypeRepository
            .FindByCondition(ct => contactTypeNames.Contains(ct.Name))
            .ToDictionaryAsync(ct => ct.Name, cancellationToken);

        return contactDtos.Select(dto =>
        {
            if (!existingContactTypes.TryGetValue(dto.ContactType!, out var contactType))
            {
                contactType = new ContactTypeEntity(dto.ContactType!);
                existingContactTypes[dto.ContactType!] = contactType;
            }

            return new ContactInfoEntity(dto.Value!, contactType);
        }).ToList();
    }

    private List<EducationEntity> BuildEducationHistory(List<EducationParseDto>? educationDtos)
    {
        if (educationDtos is null) return [];
        return educationDtos.Select(dto => new EducationEntity
        {
            Id = 0,
            Degree = dto.Degree!,
            InstitutionName = dto.InstitutionName ?? string.Empty,
            StartDate = ToDateTimeOrDefault(dto.StartDate),
            EndDate = ToNullableDateTime(dto.EndDate)
        }).ToList();
    }

    private List<WorkExperienceEntity> BuildWorkExperience(List<WorkExperienceParseDto>? experienceDtos)
    {
        if (experienceDtos is null) return [];
        return experienceDtos.Select(dto => new WorkExperienceEntity
        {
            Id = 0,
            CompanyName = dto.CompanyName ?? string.Empty,
            StartDate = ToDateTimeOrDefault(dto.StartDate),
            EndDate = ToNullableDateTime(dto.EndDate)
        }).ToList();
    }

    private SummaryEntity? BuildSummary(SummaryParseDto? summaryDto)
    {
        if (summaryDto is null) return null;

        return new SummaryEntity(summaryDto.Introduction ?? string.Empty);
    }

    private async Task<SkillSetEntity> BuildSkillSetAsync(SkillSetParseDto skillSetDto, CancellationToken cancellationToken)
    {
        var skillSetEntity = new SkillSetEntity(skillSetDto.Overview!, 0);

        if (skillSetDto.Skills is null || !skillSetDto.Skills.Any())
        {
            return skillSetEntity;
        }

        var skillNames = skillSetDto.Skills.Select(s => s.Name!).Distinct(StringComparer.OrdinalIgnoreCase).ToList();

        var existingSkillTypes = await skillTypeRepository
            .FindAsync(st => skillNames.Contains(st.Name), cancellationToken)
            .ConfigureAwait(false);

        var skills = skillSetDto.Skills.Select((skillDto, i) =>
        {
            if (!existingSkillTypes.TryGetValue(skillDto.Name!, out var skillType))
            {
                skillType = new SkillType(skillDto.Name!, "Uncategorized");
                existingSkillTypes[skillDto.Name!] = skillType;
            }

            var rank = (ushort)(i + 1);
            var level = Convert.ToUInt16(skillDto.Level);

            return new SkillEntity(level, rank, skillSetEntity, skillType);
        }).ToList();

        foreach(var skill in skills)
        {
            skillSetEntity.Add(skill);
        }

        return skillSetEntity;
    }
*/
    private static DateTime ToDateTimeOrDefault(DateOnly? date) => date?.ToDateTime(TimeOnly.MinValue) ?? default;
    private static DateTime? ToNullableDateTime(DateOnly? date) => date?.ToDateTime(TimeOnly.MinValue);
}