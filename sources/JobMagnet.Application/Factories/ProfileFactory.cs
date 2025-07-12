using CommunityToolkit.Diagnostics;
using JobMagnet.Application.UseCases.CvParser.DTO.ParsingDTOs;
using JobMagnet.Domain.Aggregates.Contact;
using JobMagnet.Domain.Aggregates.Profiles;
using JobMagnet.Domain.Aggregates.Profiles.Entities;
using JobMagnet.Domain.Aggregates.Profiles.ValueObjects;
using JobMagnet.Domain.Aggregates.SkillTypes;
using JobMagnet.Domain.Aggregates.SkillTypes.Entities;
using JobMagnet.Domain.Ports.Repositories.Base;
using JobMagnet.Domain.Services;
using JobMagnet.Shared.Abstractions;

namespace JobMagnet.Application.Factories;

public interface IProfileFactory
{
    Task<Profile> CreateProfileFromDtoAsync(ProfileParseDto profileDto, CancellationToken cancellationToken);
}

public class ProfileFactory(
    IGuidGenerator guidGenerator,
    IClock clock,
    IContactTypeResolverService contactTypeResolver,
    ISkillTypeResolverService skillTypeResolverService,
    IQueryRepository<SkillCategory, SkillCategoryId> skillCategoryRepository) : IProfileFactory
{
    private ProfileId _profileId;

    public async Task<Profile> CreateProfileFromDtoAsync(ProfileParseDto profileDto, CancellationToken cancellationToken)
    {
        Guard.IsNotNull(profileDto);

        var profile = Profile.CreateInstance(
            guidGenerator,
            clock,
            profileDto.FirstName,
            profileDto.LastName,
            profileDto.ProfileImageUrl,
            profileDto.BirthDate,
            profileDto.MiddleName,
            profileDto.SecondLastName);

        _profileId = profile.Id;

        var talents = BuildTalents(profileDto.Talents);
        var testimonials = BuildTestimonials(profileDto.Testimonials);
        var galleryItems = BuildProjects(profileDto.Project);

        foreach (var talent in talents)
            profile.TalentShowcase.AddTalent(talent.Description);

        foreach (var item in testimonials)
            profile.AddTestimonial(
                guidGenerator,
                item.Name,
                item.JobTitle,
                item.Feedback,
                item.PhotoUrl);

        foreach (var item in galleryItems)
            _ = profile.AddProject(
                guidGenerator,
                item.Title,
                item.Description,
                item.UrlLink,
                item.UrlImage,
                item.UrlVideo,
                item.Type);

        if (profileDto.Resume is not null)
        {
            profile.AddHeader(
                guidGenerator,
                profileDto.Resume.Title ?? string.Empty,
                profileDto.Resume.Suffix ?? string.Empty,
                profileDto.Resume.JobTitle ?? string.Empty,
                profileDto.Resume.About ?? string.Empty,
                profileDto.Resume.Summary ?? string.Empty,
                profileDto.Resume.Overview ?? string.Empty,
                profileDto.Resume.Address ?? string.Empty);

            foreach (var dto in profileDto.Resume.ContactInfo.Where(info => !string.IsNullOrWhiteSpace(info.ContactType)))
            {
                var resolvedType = await contactTypeResolver.ResolveAsync(dto.ContactType!, cancellationToken);

                var contactType = resolvedType.HasValue ? resolvedType.Value : ContactType.CreateInstance(guidGenerator, clock, dto.ContactType!);

                profile.AddContactInfo(guidGenerator, dto.Value!, contactType);
            }
        }

        if (profileDto.SkillSet is not null)
        {
            var skillSet = await BuildSkillSetAsync(profileDto.SkillSet, cancellationToken);
            profile.AddSkillSet(skillSet);
        }

        return profile;
    }

    private async Task<ProfileHeader> BuildHeaderAsync(ResumeParseDto resumeDto, CancellationToken cancellationToken)
    {
        var resumeEntity = ProfileHeader.CreateInstance(
            guidGenerator,
            _profileId,
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

            var contactType = resolvedType.HasValue ? resolvedType.Value : ContactType.CreateInstance(guidGenerator, clock, dto.ContactType!);

            resumeEntity.AddContactInfo(guidGenerator, dto.Value!, contactType);
        }

        return resumeEntity;
    }

    private static List<Talent> BuildTalents(List<TalentParseDto>? talentDtos)
    {
        if (talentDtos is null) return [];

        foreach (var talentDto in talentDtos)
            if (string.IsNullOrWhiteSpace(talentDto.Description))
                throw new ArgumentException("Talent description cannot be null or whitespace.", nameof(talentDtos));

        return talentDtos
            .Select(dto => Talent.CreateInstance(dto.Description ?? string.Empty)).ToList();
    }

    private List<Testimonial> BuildTestimonials(List<TestimonialParseDto>? testimonials)
    {
        if (testimonials is null) return [];

        return testimonials.Select(dto => Testimonial.CreateInstance(
                guidGenerator,
                _profileId,
                dto.Name ?? string.Empty,
                dto.JobTitle ?? string.Empty,
                dto.Feedback ?? string.Empty,
                dto.PhotoUrl ?? string.Empty))
            .ToList();
    }

    private List<Project> BuildProjects(List<ProjectParseDto>? projectDtos)
    {
        if (projectDtos is null) return [];

        return projectDtos.Select((dto, index) => Project.CreateInstance(
            guidGenerator,
            _profileId,
            dto.Title ?? string.Empty,
            dto.Description ?? string.Empty,
            dto.UrlLink ?? string.Empty,
            dto.UrlImage ?? string.Empty,
            dto.UrlVideo ?? string.Empty,
            dto.Type ?? string.Empty,
            ++index
        )).ToList();
    }

    private List<AcademicDegree> BuildEducationHistory(List<EducationParseDto>? educationDtos)
    {
        if (educationDtos is null) return [];
        return educationDtos.Select(dto => AcademicDegree.CreateInstance(
            guidGenerator,
            new CareerHistoryId(),
            dto.Degree ?? string.Empty,
            dto.InstitutionName ?? string.Empty,
            dto.InstitutionLocation ?? string.Empty,
            ToDateTimeOrDefault(dto.StartDate),
            ToNullableDateTime(dto.EndDate),
            dto.Description ?? string.Empty
        )).ToList();
    }

    private List<WorkExperience> BuildWorkExperience(List<WorkExperienceParseDto>? experienceDtos)
    {
        if (experienceDtos is null) return [];
        return experienceDtos.Select(dto => WorkExperience.CreateInstance(
            guidGenerator,
            new CareerHistoryId(),
            dto.JobTitle ?? string.Empty,
            dto.CompanyName ?? string.Empty,
            dto.CompanyLocation ?? string.Empty,
            ToDateTimeOrDefault(dto.StartDate),
            ToNullableDateTime(dto.EndDate),
            dto.Description ?? string.Empty
        )).ToList();
    }

    private CareerHistory? BuildSummary(SummaryParseDto? summaryDto)
    {
        if (summaryDto is null) return null;

        return CareerHistory.CreateInstance(
            guidGenerator,
            _profileId,
            summaryDto.Introduction ?? string.Empty
        );
    }

    private async Task<SkillSet> BuildSkillSetAsync(SkillSetParseDto skillSetDto, CancellationToken cancellationToken)
    {
        var skillSet = SkillSet.CreateInstance(
            guidGenerator,
            _profileId,
            skillSetDto.Overview ?? string.Empty
        );

        var defaultCategoryLazy = new Lazy<Task<SkillCategory>>(async () =>
            await skillCategoryRepository
                .GetByIdAsync(new SkillCategoryId(SkillCategory.DefaultCategoryId), cancellationToken)
                .ConfigureAwait(false) ?? throw new InvalidOperationException("The DefaultCategory is missing."));

        var nameSkills = skillSetDto.Skills.DistinctBy(x => x.Name).Select(x => x.Name);
        var resolvedTypes = await skillTypeResolverService.ResolveAsync(nameSkills!, cancellationToken)
            .ConfigureAwait(false);

        foreach (var skill in skillSetDto.Skills.Where(skill => !string.IsNullOrWhiteSpace(skill.Name)))
        {
            resolvedTypes.TryGetValue(skill.Name!, out var maybeSkillType);

            SkillType skillTypeToUse;

            if (maybeSkillType.HasValue)
            {
                skillTypeToUse = maybeSkillType.Value;
            }
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

            skillSet.AddSkill(
                guidGenerator,
                skill.Level.GetValueOrDefault(),
                skillTypeToUse
            );
        }

        return skillSet;
    }

    private static DateTime ToDateTimeOrDefault(DateOnly? date) => date?.ToDateTime(TimeOnly.MinValue) ?? default;
    private static DateTime? ToNullableDateTime(DateOnly? date) => date?.ToDateTime(TimeOnly.MinValue);
}