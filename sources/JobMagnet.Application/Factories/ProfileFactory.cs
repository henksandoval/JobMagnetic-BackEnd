using JobMagnet.Application.UseCases.CvParser.DTO.ParsingDTOs;
using JobMagnet.Domain.Core.Entities;
using JobMagnet.Domain.Core.Entities.Contact;
using JobMagnet.Domain.Core.Entities.Skills;
using JobMagnet.Domain.Ports.Repositories.Base;
using JobMagnet.Domain.Services;

namespace JobMagnet.Application.Factories;

public interface IProfileFactory
{
    Task<ProfileEntity> CreateProfileFromDtoAsync(ProfileParseDto profileDto, CancellationToken cancellationToken);
}

public class ProfileFactory(
    IContactTypeResolverService contactTypeResolver,
    ISkillTypeResolverService skillTypeResolverService,
    IQueryRepository<SkillCategory, ushort> skillCategoryRepository) : IProfileFactory
{
    public async Task<ProfileEntity> CreateProfileFromDtoAsync(ProfileParseDto profileDto,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(profileDto);

        var profile = new ProfileEntity
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
        var galleryItems = BuildPortfolio(profileDto.PortfolioGallery);

        profile.AddTalents(talents);

        foreach (var item in testimonials)
            profile.SocialProof.AddTestimonial(item.Name, item.JobTitle, item.Feedback, item.PhotoUrl);

        foreach (var item in galleryItems)
            profile.Portfolio.AddGallery(
                item.Description,
                item.Title,
                item.Type,
                item.UrlImage,
                item.UrlVideo,
                item.Type);

        if (profileDto.Resume is not null)
        {
            var resume = await BuildResumeAsync(profileDto.Resume, cancellationToken);
            profile.AddResume(resume);
        }

        if (profileDto.SkillSet is not null)
        {
            var skillSet = await BuildSkillSetAsync(profileDto.SkillSet, cancellationToken);
            profile.AddSkill(skillSet);
        }

        return profile;
    }

    private async Task<ResumeEntity> BuildResumeAsync(ResumeParseDto resumeDto, CancellationToken cancellationToken)
    {
        var resumeEntity = new ResumeEntity(
            resumeDto.Title ?? string.Empty,
            resumeDto.Suffix ?? string.Empty,
            resumeDto.JobTitle ?? string.Empty,
            resumeDto.About ?? string.Empty,
            resumeDto.Summary ?? string.Empty,
            resumeDto.Overview ?? string.Empty,
            resumeDto.Address ?? string.Empty);

        foreach (var dto in resumeDto.ContactInfo.Where(info => !string.IsNullOrWhiteSpace(info.ContactType)))
        {
            var resolvedType = await contactTypeResolver.ResolveAsync(dto.ContactType!, cancellationToken);

            var contactType = resolvedType.HasValue ? resolvedType.Value : new ContactType(dto.ContactType!);

            resumeEntity.AddContactInfo(dto.Value!, contactType);
        }

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

    private static List<TestimonialEntity> BuildTestimonials(List<TestimonialParseDto>? testimonials)
    {
        if (testimonials is null) return [];

        return testimonials.Select(dto => new TestimonialEntity(
            dto.Name ?? string.Empty,
            dto.JobTitle ?? string.Empty,
            dto.Feedback ?? string.Empty,
            dto.PhotoUrl ?? string.Empty)).ToList();
    }

    private List<PortfolioGalleryEntity> BuildPortfolio(List<PortfolioGalleryParseDto>? portfolioDtos)
    {
        if (portfolioDtos is null) return [];

        return portfolioDtos.Select(dto => new PortfolioGalleryEntity(
                dto.Title ?? string.Empty,
                dto.Description ?? string.Empty,
                dto.UrlLink ?? string.Empty,
                dto.UrlImage ?? string.Empty,
                dto.UrlVideo ?? string.Empty,
                dto.Type ?? string.Empty))
            .ToList();
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

    private async Task<SkillSet> BuildSkillSetAsync(SkillSetParseDto skillSetDto,
        CancellationToken cancellationToken)
    {
        var skillSetEntity = new SkillSet(skillSetDto.Overview!, 0);
        var defaultCategory = await skillCategoryRepository
            .FirstAsync(c => c.Name == SkillCategory.DefaultCategoryName, cancellationToken)
            .ConfigureAwait(false);

        foreach (var skill in skillSetDto.Skills.Where(skill => !string.IsNullOrWhiteSpace(skill.Name)))
        {
            var resolvedType = await skillTypeResolverService.ResolveAsync(skill.Name!, cancellationToken)
                .ConfigureAwait(false); //TODO: Add method by resolving skills by batch

            var skillType = resolvedType.HasValue ? resolvedType.Value : new SkillType(skill.Name!, defaultCategory);

            skillSetEntity.AddSkill(skill.Level.GetValueOrDefault(), skillType);
        }

        return skillSetEntity;
    }

    private static DateTime ToDateTimeOrDefault(DateOnly? date) => date?.ToDateTime(TimeOnly.MinValue) ?? default;
    private static DateTime? ToNullableDateTime(DateOnly? date) => date?.ToDateTime(TimeOnly.MinValue);
}