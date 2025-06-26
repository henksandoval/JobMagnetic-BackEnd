using JobMagnet.Application.UseCases.CvParser.DTO.ParsingDTOs;
using JobMagnet.Domain.Core.Entities;
using JobMagnet.Domain.Core.Entities.Contact;
using JobMagnet.Domain.Core.Entities.Skills;
using JobMagnet.Domain.Ports.Repositories.Base;
using JobMagnet.Domain.Services;

namespace JobMagnet.Application.Factories;

public interface IProfileFactory
{
    Task<Profile> CreateProfileFromDtoAsync(ProfileParseDto profileDto, CancellationToken cancellationToken);
}

public class ProfileFactory(
    IContactTypeResolverService contactTypeResolver,
    ISkillTypeResolverService skillTypeResolverService,
    IQueryRepository<SkillCategory, ushort> skillCategoryRepository) : IProfileFactory
{
    public async Task<Profile> CreateProfileFromDtoAsync(ProfileParseDto profileDto,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(profileDto);

        var profile = new Profile
        (
            profileDto.FirstName,
            profileDto.LastName,
            profileDto.ProfileImageUrl,
            profileDto.BirthDate,
            profileDto.MiddleName,
            profileDto.SecondLastName
        );

        var talents = BuildTalents(profileDto.Talents);
        var testimonials = BuildTestimonials(profileDto.Testimonials);
        var galleryItems = BuildProjects(profileDto.Project);

        foreach (var talent in talents)
            profile.TalentShowcase.AddTalent(talent.Description);

        foreach (var item in testimonials)
            profile.SocialProof.AddTestimonial(item.Name, item.JobTitle, item.Feedback, item.PhotoUrl);

        foreach (var item in galleryItems)
            profile.Portfolio.AddProject(
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

    private async Task<Resume> BuildResumeAsync(ResumeParseDto resumeDto, CancellationToken cancellationToken)
    {
        var resumeEntity = new Resume(
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

    private List<Talent> BuildTalents(List<TalentParseDto>? talentDtos)
    {
        if (talentDtos is null) return new List<Talent>();

        foreach (var talentDto in talentDtos)
            if (string.IsNullOrWhiteSpace(talentDto.Description))
                throw new ArgumentException("Talent description cannot be null or whitespace.", nameof(talentDtos));

        return talentDtos
            .Select(dto => new Talent(
                dto.Description!
            ))
            .ToList();
    }

    private static List<Testimonial> BuildTestimonials(List<TestimonialParseDto>? testimonials)
    {
        if (testimonials is null) return [];

        return testimonials.Select(dto => new Testimonial(
            dto.Name ?? string.Empty,
            dto.JobTitle ?? string.Empty,
            dto.Feedback ?? string.Empty,
            dto.PhotoUrl ?? string.Empty)).ToList();
    }

    private List<Project> BuildProjects(List<ProjectParseDto>? projectDtos)
    {
        if (projectDtos is null) return [];

        return projectDtos.Select((dto, index) => new Project(
                dto.Title ?? string.Empty,
                dto.Description ?? string.Empty,
                dto.UrlLink ?? string.Empty,
                dto.UrlImage ?? string.Empty,
                dto.UrlVideo ?? string.Empty,
                dto.Type ?? string.Empty,
                ++index
            ))
            .ToList();
    }

    private List<EducationEntity> BuildEducationHistory(List<EducationParseDto>? educationDtos)
    {
        if (educationDtos is null) return [];
        return educationDtos.Select(dto => new EducationEntity(
            dto.Degree ?? string.Empty,
            dto.InstitutionName ?? string.Empty,
            dto.InstitutionLocation ?? string.Empty,
            ToDateTimeOrDefault(dto.StartDate),
            ToNullableDateTime(dto.EndDate),
            dto.Description ?? string.Empty
        )).ToList();
    }

    private List<WorkExperienceEntity> BuildWorkExperience(List<WorkExperienceParseDto>? experienceDtos)
    {
        if (experienceDtos is null) return [];
        return experienceDtos.Select(dto => new WorkExperienceEntity(
            dto.JobTitle ?? string.Empty,
            dto.CompanyName ?? string.Empty,
            dto.CompanyLocation ?? string.Empty,
            ToDateTimeOrDefault(dto.StartDate),
            ToNullableDateTime(dto.EndDate),
            dto.Description ?? string.Empty
        )).ToList();
    }

    private ProfessionalSummary? BuildSummary(SummaryParseDto? summaryDto)
    {
        if (summaryDto is null) return null;

        return new ProfessionalSummary(summaryDto.Introduction ?? string.Empty);
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