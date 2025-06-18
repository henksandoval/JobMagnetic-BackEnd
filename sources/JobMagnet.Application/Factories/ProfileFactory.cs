using JobMagnet.Application.UseCases.CvParser.DTO.ParsingDTOs;
using JobMagnet.Application.UseCases.CvParser.DTO.RawDTOs;
using JobMagnet.Domain.Core.Entities;
using JobMagnet.Domain.Ports.Repositories.Base;
using JobMagnet.Domain.Services;

namespace JobMagnet.Application.Factories;

public interface IProfileFactory
{
    Task<ProfileEntity> CreateProfileFromDtoAsync(ProfileParseDto profileDto, CancellationToken cancellationToken);
}

public class ProfileFactory(
    IQueryRepository<SkillType, int> skillTypeRepository,
    IContactTypeResolverService contactTypeResolver) : IProfileFactory
{
    public async Task<ProfileEntity> CreateProfileFromDtoAsync(ProfileParseDto profileDto,
        CancellationToken cancellationToken)
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

        var talents = BuildTalents(profileDto.Talents);
        var testimonials = BuildTestimonials(profileDto.Testimonials);
        var portfolio = BuildPortfolio(profileDto.PortfolioGallery);

        profileEntity.AddTalents(talents);
        profileEntity.AddTestimonials(testimonials);
        profileEntity.AddPortfolioItems(portfolio);

        if (profileDto.Resume is not null)
        {
            var resume = await BuildResumeAsync(profileDto.Resume, cancellationToken);
            profileEntity.AddResume(resume);
        }

/*

        if (profileDto.SkillSet is not null)
        {
            profileEntity.AddSkill(await BuildSkillSetAsync(profileDto.SkillSet, cancellationToken));
        }
*/
        return profileEntity;
    }

    private async Task<ResumeEntity> BuildResumeAsync(ResumeParseDto resumeDto, CancellationToken cancellationToken)
    {
        var resumeEntity = new ResumeEntity
        {
            Id = 0,
            ProfileId = 0,
            About = resumeDto.About!,
            Overview = resumeDto.Overview!,
            JobTitle = resumeDto.JobTitle!,
            Address = resumeDto.Address!,
            Suffix = resumeDto.Suffix!,
            Summary = resumeDto.Summary!,
            Title = resumeDto.Title!
        };

        var contactInfo = await BuildContactInfoCollectionAsync(resumeDto.ContactInfo, cancellationToken);

        resumeEntity.AddContactInfo(contactInfo);

        return resumeEntity;
    }

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

    private async Task<List<ContactInfoEntity>> BuildContactInfoCollectionAsync(
        List<ContactInfoParseDto>? contactInfoDtos,
        CancellationToken cancellationToken)
    {
        if (contactInfoDtos is null || contactInfoDtos.Count == 0)
        {
            return [];
        }

        var contactInfoResult = new List<ContactInfoEntity>();

        foreach (var dto in contactInfoDtos.Where(info => !string.IsNullOrWhiteSpace(info.ContactType)))
        {
            var resolvedType = await contactTypeResolver.ResolveAsync(dto.ContactType!, cancellationToken);

            var contactType = resolvedType.HasValue ? resolvedType.Value : new ContactTypeEntity(dto.ContactType!);
            if (!resolvedType.HasValue)
                contactType.SetDefaultIcon();

            contactInfoResult.Add(new ContactInfoEntity(0, dto.Value!, contactType));
        }

        return contactInfoResult;
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

        return new SummaryEntity {
            Id = 0,
            Introduction = summaryDto.Introduction ?? string.Empty
        };
    }

    private async Task<SkillSetEntity> BuildSkillSetAsync(SkillSetParseDto skillSetDto,
        CancellationToken cancellationToken)
    {
        var skillSetEntity = new SkillSetEntity(skillSetDto.Overview!, 0);

        if (skillSetDto.Skills is null || !skillSetDto.Skills.Any())
        {
            return skillSetEntity;
        }

        var skillNames = skillSetDto.Skills.Select(s => s.Name!).Distinct(StringComparer.OrdinalIgnoreCase).ToList();

        var existingSkillTypes = (await skillTypeRepository
            .FindAsync(st => skillNames.Contains(st.Name), cancellationToken)
            .ConfigureAwait(false))
            .ToDictionary(s => s.Name);

        var skills = skillSetDto.Skills.Select((skillDto, i) =>
        {
            if (!existingSkillTypes.TryGetValue(skillDto.Name!, out var skillType))
            {
                skillType = new SkillType(0, skillDto.Name!, new SkillCategory(""));
                existingSkillTypes[skillDto.Name!] = skillType;
            }

            var rank = (ushort)(i + 1);
            var level = Convert.ToUInt16(skillDto.Level);

            return new SkillEntity(level, rank, skillSetEntity, skillType);
        }).ToList();

        foreach (var skill in skills)
        {
            skillSetEntity.Add(skill);
        }

        return skillSetEntity;
    }

    private static DateTime ToDateTimeOrDefault(DateOnly? date) => date?.ToDateTime(TimeOnly.MinValue) ?? default;
    private static DateTime? ToNullableDateTime(DateOnly? date) => date?.ToDateTime(TimeOnly.MinValue);
}