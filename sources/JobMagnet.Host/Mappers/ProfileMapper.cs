using JobMagnet.Domain.Core.Entities;
using JobMagnet.Domain.Core.Entities.Skills;
using JobMagnet.Host.ViewModels.Profile;
using JobMagnet.Shared.Utils;
using Mapster;

namespace JobMagnet.Host.Mappers;

public static class ProfileMapper
{
    static ProfileMapper()
    {
        ConfigMapper();
    }

    public static ProfileViewModel ToViewModel(this ProfileEntity entity) => entity.Adapt<ProfileViewModel>();

    private static void ConfigMapper()
    {
        TypeAdapterConfig<ProfileEntity, ProfileViewModel>
            .NewConfig()
            .Map(dest => dest.PersonalData, src => PersonalDataViewModelMap(src))
            .Map(dest => dest.About, src => src.Adapt<AboutViewModel>())
            .Map(dest => dest.Testimonials,
                src => src.Testimonials.Select(t => t.Adapt<TestimonialsViewModel>()).ToArray(),
                src => src.Testimonials.Any())
            .Map(dest => dest.Project,
                src => src.Projects.Select(p => p.Adapt<ProjectViewModel>()).ToArray(),
                src => src.Projects.Any())
            .Map(dest => dest.SkillSet, src => src.SkillSet.Adapt<SkillSetViewModel>(),
                src => src.SkillSet != null && src.SkillSet.Skills.Count > 0)
            .Map(dest => dest.Summary, src => src.Summary.Adapt<SummaryViewModel>(),
                src => src.Summary != null);

        TypeAdapterConfig<Project, ProjectViewModel>
            .NewConfig()
            .Map(dest => dest.Image, src => src.UrlImage)
            .Map(dest => dest.Link, src => src.UrlLink)
            .Map(dest => dest.Video, src => src.UrlVideo);

        TypeAdapterConfig<TestimonialEntity, TestimonialsViewModel>
            .NewConfig()
            .Map(dest => dest.Testimonial, src => src.Feedback);

        TypeAdapterConfig<ProfileEntity, AboutViewModel>
            .NewConfig()
            .Map(dest => dest, src => AboutViewModelMap(src));

        TypeAdapterConfig<Skill, SkillDetailsViewModel>
            .NewConfig()
            .Map(dest => dest.Name, src => src.SkillType.Name)
            .Map(dest => dest.IconUrl, src => src.SkillType.IconUrl);

        TypeAdapterConfig<SkillSet, SkillSetViewModel>
            .NewConfig()
            .Map(dest => dest.SkillDetails,
                src => src.Skills.Select(d => d.Adapt<SkillDetailsViewModel>()).ToArray());

        TypeAdapterConfig<SummaryEntity, SummaryViewModel>
            .NewConfig()
            .Map(dest => dest.Education, src => EducationViewModelMap(src))
            .Map(dest => dest.WorkExperience, src => WorkExperienceViewModelMap(src));
    }

    private static AboutViewModel AboutViewModelMap(ProfileEntity entity)
    {
        ArgumentException.ThrowIfNullOrEmpty(nameof(ProfileEntity), "ProfileEntity cannot be null.");

        var viewModel = new AboutViewModel(
            entity.ProfileImageUrl ?? string.Empty,
            entity.Resume?.About ?? string.Empty,
            entity.Resume?.JobTitle ?? string.Empty,
            entity.Resume?.Overview ?? string.Empty,
            entity.BirthDate,
            GetContactValue(entity, "Website"),
            GetContactValue(entity, "Phone"),
            entity.Resume?.Address ?? string.Empty,
            entity.BirthDate.GetAge(),
            entity.Resume?.Title ?? string.Empty,
            GetContactValue(entity, "Email"),
            entity.Resume?.Summary ?? string.Empty,
            entity.Resume?.Summary ?? string.Empty
        );
        return viewModel;
    }

    private static EducationViewModel EducationViewModelMap(SummaryEntity src)
    {
        var academicBackground = src.Education?.Select(e => new AcademicBackgroundViewModel(
            e.Degree,
            e.StartDate.ToString("yyyy-MM-dd"),
            e.InstitutionName,
            e.Description
        )).ToArray() ?? [];

        return new EducationViewModel(academicBackground);
    }

    private static WorkExperienceViewModel WorkExperienceViewModelMap(SummaryEntity src)
    {
        var workExperienceList = src.WorkExperiences?.Select(work =>
            {
                var responsibilities = work.Responsibilities?
                    .Select(r => r.Description)
                    .ToArray() ?? [];
                return new PositionViewModel(
                    work.JobTitle,
                    work.StartDate.ToString("yyyy-MM-dd"),
                    work.CompanyLocation,
                    work.Description,
                    responsibilities);
            })
            .ToArray() ?? [];
        return new WorkExperienceViewModel(workExperienceList);
    }

    private static PersonalDataViewModel PersonalDataViewModelMap(ProfileEntity src)
    {
        var socialNetworks = src.Resume?.ContactInfo?.Select(c => new SocialNetworksViewModel(
                c.ContactType.Name,
                c.Value,
                c.ContactType.IconClass ?? string.Empty,
                c.ContactType.IconUrl?.AbsoluteUri ?? string.Empty))
            .ToArray() ?? [];

        var professions = src.Talents?.Select(t => t.Description).ToArray() ?? [];

        return new PersonalDataViewModel(
            GetFullName(src),
            professions,
            socialNetworks
        );
    }

    private static string GetFullName(ProfileEntity entity)
    {
        return string.Join(" ", new[] { entity.FirstName, entity.MiddleName, entity.LastName, entity.SecondLastName }
            .Where(x => !string.IsNullOrWhiteSpace(x)));
    }

    private static string GetContactValue(ProfileEntity entity, string contactTypeName)
    {
        return entity.Resume?.ContactInfo?
            .FirstOrDefault(c => string.Equals(c.ContactType.Name, contactTypeName, StringComparison.OrdinalIgnoreCase))
            ?.Value ?? string.Empty;
    }
}