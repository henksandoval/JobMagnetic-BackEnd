using JobMagnet.Domain.Aggregates.Profiles;
using JobMagnet.Domain.Aggregates.Profiles.Entities;
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

    public static ProfileViewModel ToViewModel(this Profile entity) => entity.Adapt<ProfileViewModel>();

    private static void ConfigMapper()
    {
        TypeAdapterConfig<Profile, ProfileViewModel>
            .NewConfig()
            .Map(dest => dest.PersonalData, src => PersonalDataViewModelMap(src))
            .Map(dest => dest.About, src => src.Adapt<AboutViewModel>())
            .Map(dest => dest.Testimonials,
                src => src.Testimonials.Select(t => t.Adapt<TestimonialsViewModel>()).ToArray(),
                src => src.Testimonials.Any())
            .Map(dest => dest.Project,
                src => src.Portfolio.Select(p => p.Adapt<ProjectViewModel>()).ToArray(),
                src => src.Portfolio.Any())
            .Map(dest => dest.SkillSet, src => src.SkillSet.Adapt<SkillSetViewModel>(),
                src => src.HaveSkillSet)
            .Map(dest => dest.Summary, src => src.CareerHistory.Adapt<SummaryViewModel>(),
                src => src.CareerHistory != null);

        TypeAdapterConfig<Project, ProjectViewModel>
            .NewConfig()
            .Map(dest => dest.Image, src => src.UrlImage)
            .Map(dest => dest.Link, src => src.UrlLink)
            .Map(dest => dest.Video, src => src.UrlVideo);

        TypeAdapterConfig<Testimonial, TestimonialsViewModel>
            .NewConfig()
            .Map(dest => dest.Testimonial, src => src.Feedback);

        TypeAdapterConfig<Profile, AboutViewModel>
            .NewConfig()
            .Map(dest => dest, src => AboutViewModelMap(src));

        TypeAdapterConfig<Skill, SkillDetailsViewModel>
            .NewConfig()
            .Map(dest => dest.Name, src => src.SkillType.Name)
            .Map(dest => dest.Rank, src => src.Position)
            .Map(dest => dest.IconUrl, src => src.SkillType.IconUrl);

        TypeAdapterConfig<SkillSet, SkillSetViewModel>
            .NewConfig()
            .Map(dest => dest.SkillDetails,
                src => src.Skills.Select(d => d.Adapt<SkillDetailsViewModel>()).ToArray());

        TypeAdapterConfig<CareerHistory, SummaryViewModel>
            .NewConfig()
            .Map(dest => dest.Education, src => EducationViewModelMap(src))
            .Map(dest => dest.WorkExperience, src => WorkExperienceViewModelMap(src));
    }

    private static AboutViewModel AboutViewModelMap(Profile entity)
    {
        ArgumentException.ThrowIfNullOrEmpty(nameof(Profile), "Profile cannot be null.");

        var viewModel = new AboutViewModel(
            entity.ProfileImageUrl ?? string.Empty,
            entity.ProfileHeader?.About ?? string.Empty,
            entity.ProfileHeader?.JobTitle ?? string.Empty,
            entity.ProfileHeader?.Overview ?? string.Empty,
            entity.BirthDate,
            GetContactValue(entity, "Website"),
            GetContactValue(entity, "Phone"),
            entity.ProfileHeader?.Address ?? string.Empty,
            entity.BirthDate.GetAge(),
            entity.ProfileHeader?.Title ?? string.Empty,
            GetContactValue(entity, "Email"),
            entity.ProfileHeader?.Summary ?? string.Empty,
            entity.ProfileHeader?.Summary ?? string.Empty
        );
        return viewModel;
    }

    private static EducationViewModel EducationViewModelMap(CareerHistory src)
    {
        var academicBackground = src.Qualifications?.Select(e => new AcademicBackgroundViewModel(
            e.Degree,
            e.StartDate.ToString("yyyy-MM-dd"),
            e.InstitutionName,
            e.Description
        )).ToArray() ?? [];

        return new EducationViewModel(academicBackground);
    }

    private static WorkExperienceViewModel WorkExperienceViewModelMap(CareerHistory src)
    {
        var workExperienceList = src.WorkExperiences?.Select(work =>
            {
                var responsibilities = work.Highlights?
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

    private static PersonalDataViewModel PersonalDataViewModelMap(Profile src)
    {
        var socialNetworks = src.ProfileHeader?.ContactInfo?.Select(c => new SocialNetworksViewModel(
                c.ContactType.Name,
                c.Value,
                c.ContactType.IconClass ?? string.Empty,
                c.ContactType.IconUrl?.AbsoluteUri ?? string.Empty))
            .ToArray() ?? [];

        var professions = src.TalentShowcase?.Select(t => t.Description).ToArray() ?? [];

        return new PersonalDataViewModel(
            GetFullName(src),
            professions,
            socialNetworks
        );
    }

    private static string GetFullName(Profile entity)
    {
        return string.Join(" ", new[] { entity.FirstName, entity.MiddleName, entity.LastName, entity.SecondLastName }
            .Where(x => !string.IsNullOrWhiteSpace(x)));
    }

    private static string GetContactValue(Profile entity, string contactTypeName)
    {
        return entity.ProfileHeader?.ContactInfo?
            .FirstOrDefault(c => string.Equals(c.ContactType.Name, contactTypeName, StringComparison.OrdinalIgnoreCase))
            ?.Value ?? string.Empty;
    }
}