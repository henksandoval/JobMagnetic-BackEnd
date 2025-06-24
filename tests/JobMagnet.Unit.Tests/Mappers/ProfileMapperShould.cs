using AutoFixture;
using JobMagnet.Domain.Core.Entities;
using JobMagnet.Host.Mappers;
using JobMagnet.Host.ViewModels.Profile;
using JobMagnet.Shared.Tests.Fixtures;
using JobMagnet.Shared.Tests.Fixtures.Builders;
using JobMagnet.Shared.Utils;
using Shouldly;

namespace JobMagnet.Unit.Tests.Mappers;

public class ProfileMapperShould
{
    private readonly IFixture _fixture = FixtureBuilder.Build();

    [Fact(DisplayName = "Map ProfileEntity to ProfileViewModel when PersonalData is defined")]
    public void MapperProfileEntityToProfileViewModelWithPersonalData()
    {
        var profileBuilder = new ProfileEntityBuilder(_fixture)
            .WithTalents()
            .WithResume()
            .WithContactInfo();

        var profile = profileBuilder.Build();
        profile.SecondLastName = string.Empty;
        profile.MiddleName = string.Empty;

        var profileExpected = new ProfileViewModel();

        profileExpected = profileExpected with { PersonalData = GetPersonalDataViewModel(profile) };

        var result = profile.ToViewModel();

        result.ShouldNotBeNull();
        result.ShouldBeOfType<ProfileViewModel>();

        result.PersonalData!.ShouldBeEquivalentTo(profileExpected.PersonalData);
    }

    [Fact(DisplayName = "Map ProfileEntity to ProfileViewModel when About is defined")]
    public void MapperProfileEntityToProfileViewModelWithAbout()
    {
        var profileBuilder = new ProfileEntityBuilder(_fixture)
            .WithResume()
            .WithContactInfo();

        var profile = profileBuilder.Build();

        var profileExpected = new ProfileViewModel();

        profileExpected = profileExpected with { About = GetAboutViewModel(profile) };

        var result = profile.ToViewModel();

        result.ShouldNotBeNull();
        result.ShouldBeOfType<ProfileViewModel>();

        result.About!.ShouldBeEquivalentTo(profileExpected.About);
    }

    [Fact(DisplayName = "Map ProfileEntity to ProfileViewModel when Testimonial is defined")]
    public void MapperProfileEntityToProfileViewModelWithTestimonials()
    {
        var profile = new ProfileEntityBuilder(_fixture)
            .WithTestimonials()
            .Build();

        var profileExpected = new ProfileViewModel();

        profileExpected = profileExpected with { Testimonials = GetTestimonialViewModel(profile) };

        var result = profile.ToViewModel();

        result.ShouldNotBeNull();
        result.ShouldBeOfType<ProfileViewModel>();

        result.Testimonials!.ShouldBeEquivalentTo(profileExpected.Testimonials);
    }

    [Fact(DisplayName = "Map ProfileEntity to ProfileViewModel when Service is defined")]
    public void MapperProfileEntityToProfileViewModelWithService()
    {
        var profile = new ProfileEntityBuilder(_fixture)
            .WithServices()
            .Build();

        var profileExpected = new ProfileViewModel();

        profileExpected = profileExpected with { Service = GetServiceViewModel(profile) };

        var result = profile.ToViewModel();

        result.ShouldNotBeNull();
        result.ShouldBeOfType<ProfileViewModel>();

        result.Service!.ShouldBeEquivalentTo(profileExpected.Service);
    }

    [Fact(DisplayName = "Map ProfileEntity to ProfileViewModel when PortfolioGallery is defined")]
    public void MapperProfileEntityToProfileViewModelWithPortfolioGallery()
    {
        var profile = new ProfileEntityBuilder(_fixture)
            .WithPortfolio()
            .Build();

        var profileExpected = new ProfileViewModel();

        profileExpected = profileExpected with { PortfolioGallery = GetPortfolioViewModel(profile) };

        var result = profile.ToViewModel();

        result.ShouldNotBeNull();
        result.ShouldBeOfType<ProfileViewModel>();

        result.PortfolioGallery.ShouldBeEquivalentTo(profileExpected.PortfolioGallery);
    }

    [Fact(DisplayName = "Map ProfileEntity to ProfileViewModel when Skills are defined")]
    public void MapperProfileEntityToProfileViewModelWithSkills()
    {
        var profile = new ProfileEntityBuilder(_fixture)
            .WithSkillSet()
            .WithSkills()
            .Build();

        var profileExpected = new ProfileViewModel();

        profileExpected = profileExpected with { SkillSet = GetSkillViewModel(profile) };

        var result = profile.ToViewModel();

        result.ShouldNotBeNull();
        result.ShouldBeOfType<ProfileViewModel>();

        result.SkillSet!.ShouldBeEquivalentTo(profileExpected.SkillSet);
    }

    [Fact(DisplayName = "Map ProfileEntity to ProfileViewModel when SummaryViewModel is defined")]
    public void MapperProfileEntityToSummaryViewModelWithPortfolioGallery()
    {
        var profile = new ProfileEntityBuilder(_fixture)
            .WithSummary()
            .Build();

        var profileExpected = new ProfileViewModel();

        profileExpected = profileExpected with { Summary = GetSummaryViewModel(profile) };

        var result = profile.ToViewModel();

        result.ShouldNotBeNull();
        result.ShouldBeOfType<ProfileViewModel>();

        result.Summary.ShouldBeEquivalentTo(profileExpected.Summary);
    }

    private static PersonalDataViewModel GetPersonalDataViewModel(ProfileEntity entity)
    {
        return new PersonalDataViewModel(
            $"{entity.FirstName} {entity.LastName}",
            entity.Talents.Select(x => x.Description).ToArray(),
            entity.Resume.ContactInfo.Select(c => new SocialNetworksViewModel(
                c.ContactType.Name,
                c.Value,
                c.ContactType.IconClass ?? string.Empty,
                c.ContactType.IconUrl?.AbsoluteUri ?? string.Empty
            )).ToArray()
        );
    }

    private static AboutViewModel GetAboutViewModel(ProfileEntity entity)
    {
        var webSite = GetContactValue("Website");
        var email = GetContactValue("Email");
        var mobilePhone = GetContactValue("Phone");

        return new AboutViewModel(
            entity.ProfileImageUrl,
            entity.Resume.About,
            entity.Resume.JobTitle,
            entity.Resume.Overview,
            entity.BirthDate!.Value,
            webSite,
            mobilePhone,
            entity.Resume.Address!,
            entity.BirthDate.GetAge(),
            entity.Resume.Title ?? string.Empty,
            email,
            entity.Resume.Summary ?? string.Empty,
            entity.Resume.Summary
        );

        string GetContactValue(string contactTypeName)
        {
            return entity.Resume.ContactInfo
                .FirstOrDefault(x => x.ContactType.Name == contactTypeName)
                ?.Value ?? string.Empty;
        }
    }

    private static ServiceViewModel GetServiceViewModel(ProfileEntity profile)
    {
        var serviceDetails = profile.Services.GalleryItems.Select(g => new ServiceDetailsViewModel(
                g.Title,
                g.Description,
                g.UrlImage))
            .ToArray();

        return new ServiceViewModel(profile.Services.Overview, serviceDetails);
    }

    private static TestimonialsViewModel[]? GetTestimonialViewModel(ProfileEntity profile)
    {
        return profile.Testimonials.Select(t => new TestimonialsViewModel(
                t.Name,
                t.JobTitle,
                t.PhotoUrl,
                t.Feedback))
            .ToArray();
    }

    private static PortfolioViewModel[]? GetPortfolioViewModel(ProfileEntity profile)
    {
        return profile.PortfolioGallery.Select(p => new PortfolioViewModel(
                p.Position,
                p.Title,
                p.Description,
                p.UrlLink,
                p.UrlImage,
                p.Type,
                p.UrlVideo))
            .ToArray();
    }

    private static SkillSetViewModel GetSkillViewModel(ProfileEntity profile)
    {
        var skills = profile.SkillSet.Skills
            .Select(skill => new SkillDetailsViewModel(skill.SkillType.Name, skill.SkillType.IconUrl.AbsoluteUri, skill.Rank))
            .ToArray();

        return new SkillSetViewModel(profile.SkillSet.Overview!, skills);
    }

    private static SummaryViewModel GetSummaryViewModel(ProfileEntity profile)
    {
        var education = profile.Summary.Education
            .Select(e => new AcademicBackgroundViewModel(
                e.Degree,
                e.StartDate.ToString("yyyy-MM-dd"),
                e.InstitutionName,
                e.Description))
            .ToArray();

        var workExperiences = profile.Summary.WorkExperiences
            .Select(w => new PositionViewModel(
                w.JobTitle,
                w.StartDate.ToString("yyyy-MM-dd"),
                w.CompanyLocation,
                w.Description,
                w.Responsibilities.Select(d => d.Description).ToArray()))
            .ToArray();

        return new SummaryViewModel(
            profile.Summary.Introduction,
            new EducationViewModel(education),
            new WorkExperienceViewModel(workExperiences));
    }
}