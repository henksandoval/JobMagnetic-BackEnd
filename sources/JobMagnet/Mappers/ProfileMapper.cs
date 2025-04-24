using JobMagnet.Extensions.Utils;
using JobMagnet.Infrastructure.Entities;
using JobMagnet.ViewModels.Profile;
using Mapster;

// ReSharper disable ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
namespace JobMagnet.Mappers;

internal static class ProfileMapper
{
    static ProfileMapper()
    {
        TypeAdapterConfig<PortfolioGalleryEntity, PortfolioViewModel>
            .NewConfig()
            .Map(dest => dest.Image, src => src.UrlImage)
            .Map(dest => dest.Link, src => src.UrlLink)
            .Map(dest => dest.Video, src => src.UrlVideo);

        TypeAdapterConfig<TestimonialEntity, TestimonialsViewModel>
            .NewConfig()
            .Map(dest => dest.Testimonial, src => src.Feedback);

        TypeAdapterConfig<ProfileEntity, AboutViewModel>
            .NewConfig()
            .Map(dest => dest.ImageUrl, src => src.ProfileImageUrl)
            .Map(dest => dest.Description, src => src.Resume.About)
            .Map(dest => dest.Text, src => src.Resume.JobTitle)
            .Map(dest => dest.Hobbies, src => src.Resume.Overview)
            .Map(dest => dest.Birthday, src => src.BirthDate)
            .Map(dest => dest.Website, src => GetContactValue(src, "Website"))
            .Map(dest => dest.PhoneNumber, src => GetContactValue(src, "Mobile Phone"))
            .Map(dest => dest.Email, src => GetContactValue(src, "Email"))
            .Map(dest => dest.City, src => src.Resume.Address)
            .Map(dest => dest.Age, src => src.BirthDate.GetAge())
            .Map(dest => dest.Degree, src => src.Resume.Title ?? string.Empty)
            .Map(dest => dest.WorkExperience, src => src.Resume.Summary)
            .Map(dest => dest.Freelance, src => string.Empty);

        TypeAdapterConfig<ProfileEntity, SkillSetViewModel>
            .NewConfig()
            .Map(dest => dest.Overview, src => src.Skill.Overview)
            .Map(dest => dest.SkillDetails,
                src => src.Skill.SkillDetails.Select(d => d.Adapt<SkillDetailsViewModel>()).ToArray());

        TypeAdapterConfig<ProfileEntity, ProfileViewModel>
            .NewConfig()
            .Map(dest => dest.PersonalData,
                src => new PersonalDataViewModel(
                    GetFullName(src),
                    src.Talents.Select(t => t.Description).ToArray(),
                    src.Resume.ContactInfo.Select(c => new SocialNetworksViewModel(
                            c.ContactType.Name,
                            c.Value,
                            c.ContactType.IconClass ?? string.Empty,
                            c.ContactType.IconUrl ?? string.Empty))
                        .ToArray()),
                src => src.Resume != null && src.Resume.ContactInfo.Any() && src.Talents.Any())
            .Map(dest => dest.About, src => src.Adapt<AboutViewModel>(),
                src => src.Resume != null && src.BirthDate != null)
            .Map(dest => dest.Testimonials,
                src => src.Testimonials.Select(t => t.Adapt<TestimonialsViewModel>()).ToArray(),
                src => src.Testimonials.Any())
            .Map(dest => dest.PortfolioGallery,
                src => src.PortfolioGallery.Select(p => p.Adapt<PortfolioViewModel>()).ToArray(),
                src => src.PortfolioGallery.Any())
            .Map(dest => dest.SkillSet, src => src.Skill.Adapt<SkillSetViewModel>(),
                src => src.Skill != null && src.Skill.SkillDetails.Count != 0)
            .Map(dest => dest.Summary,src => new SummaryViewModel(
                    src.Summaries.Introduction,
        new EducationViewModel(
                    src.Summaries.Education
                        .Select(e => new AcademicBackgroundViewModel(
                            e.Degree,
                            e.StartDate.ToString("yyyy-MM-dd"),
                            e.InstitutionName,
                            e.Description
                        )).ToArray()
                ),
                new WorkExperienceViewModel(
                    src.Summaries.WorkExperiences
                        .Select(w => new PositionViewModel(
                            w.JobTitle,
                            w.StartDate.ToString("yyyy-MM-dd"),
                            w.CompanyLocation,
                            string.Join(", ", w.Responsibilities),
                            string.Join(", ", w.Responsibilities),
                            string.Join(", ", w.Responsibilities),
                            w.Description))
                        .ToArray()
                )))
            .Map(dest => dest.Service,
                src => src.Services
                    .Select(s => new ServiceViewModel(
                        s.Overview,
                        s.GalleryItems.Select(g => new ServiceDetailsViewModel(
                                g.Title,
                                g.Description,
                                g.UrlImage))
                            .ToArray()))
                    .FirstOrDefault()
            );

    }

    internal static ProfileViewModel ToModel(ProfileEntity entity)
    {
        return entity.Adapt<ProfileViewModel>();
    }

    private static string GetFullName(ProfileEntity entity) =>
        string.Join(" ", new[] { entity.FirstName, entity.MiddleName, entity.LastName, entity.SecondLastName }
            .Where(x => !string.IsNullOrWhiteSpace(x)));

    private static string GetContactValue(ProfileEntity entity, string contactTypeName)
    {
        return entity.Resume.ContactInfo
            .FirstOrDefault(c => c.ContactType.Name == contactTypeName)
            ?.Value ?? string.Empty;
    }
}