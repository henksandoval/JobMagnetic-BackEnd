using AutoFixture;
using AwesomeAssertions;
using JobMagnet.Application.Mappers;
using JobMagnet.Domain.Aggregates.Contact;
using JobMagnet.Domain.Aggregates.Profiles;
using JobMagnet.Domain.Aggregates.Skills;
using JobMagnet.Domain.Aggregates.SkillTypes.Entities;
using JobMagnet.Host.Mappers;
using JobMagnet.Host.ViewModels.Profile;
using JobMagnet.Infrastructure.Persistence.Seeders.Collections;
using JobMagnet.Shared.Tests.Abstractions;
using JobMagnet.Shared.Tests.Fixtures;
using JobMagnet.Shared.Tests.Fixtures.Builders;
using JobMagnet.Shared.Utils;

namespace JobMagnet.Unit.Tests.Mappers;

public class ProfileMapperShould
{
    private readonly IFixture _fixture = FixtureBuilder.Build();

    [Fact(DisplayName = "Map Profile to ProfileResponse when is defined")]
    public void MapperProfileToProfileResponseWithSimpleData()
    {
        var profile = _fixture.Create<Profile>();

        var profileResponse = profile.ToModel();

        profileResponse.Should().NotBeNull();
        profileResponse.Id.Should().Be(profile.Id.Value);
        profileResponse.ProfileData.Should().BeEquivalentTo(profile, options => options.ExcludingMissingMembers());
    }

    [Fact(DisplayName = "Map Profile to ProfileViewModel when PersonalData is defined")]
    public void MapperProfileEntityToProfileViewModelWithPersonalData()
    {
        var contactTypes = GetContactTypes();
        var profileBuilder = new ProfileEntityBuilder(_fixture)
            .WithTalents()
            .WithResume()
            .WithContactInfo(contactTypes);

        var profile = profileBuilder.Build();
        profile.ChangeMiddleName(string.Empty);
        profile.ChangeSecondLastName(string.Empty);

        var profileExpected = new ProfileViewModel();

        profileExpected = profileExpected with { PersonalData = GetPersonalDataViewModel(profile) };

        var result = profile.ToViewModel();

        result.Should().NotBeNull();
        result.Should().BeOfType<ProfileViewModel>();

        result.PersonalData!.Should().BeEquivalentTo(profileExpected.PersonalData);
    }

    [Fact(DisplayName = "Map Profile to ProfileViewModel when About is defined")]
    public void MapperProfileEntityToProfileViewModelWithAbout()
    {
        var contactTypes = GetContactTypes();
        var profileBuilder = new ProfileEntityBuilder(_fixture)
            .WithResume()
            .WithContactInfo(contactTypes);

        var profile = profileBuilder.Build();

        var profileExpected = new ProfileViewModel();

        profileExpected = profileExpected with { About = GetAboutViewModel(profile) };

        var result = profile.ToViewModel();

        result.Should().NotBeNull();
        result.Should().BeOfType<ProfileViewModel>();

        result.About!.Should().BeEquivalentTo(profileExpected.About);
    }

    [Fact(DisplayName = "Map Profile to ProfileViewModel when Testimonial is defined")]
    public void MapperProfileEntityToProfileViewModelWithTestimonials()
    {
        var profile = new ProfileEntityBuilder(_fixture)
            .WithTestimonials()
            .Build();

        var profileExpected = new ProfileViewModel();

        profileExpected = profileExpected with { Testimonials = GetTestimonialViewModel(profile) };

        var result = profile.ToViewModel();

        result.Should().NotBeNull();
        result.Should().BeOfType<ProfileViewModel>();

        result.Testimonials!.Should().BeEquivalentTo(profileExpected.Testimonials);
    }

    [Fact(DisplayName = "Map Profile to ProfileViewModel when Project is defined")]
    public void MapperProfileEntityToProfileViewModelWithProject()
    {
        var profile = new ProfileEntityBuilder(_fixture)
            .WithProjects()
            .Build();

        var profileExpected = new ProfileViewModel();

        profileExpected = profileExpected with { Project = GetProjectViewModel(profile) };

        var result = profile.ToViewModel();

        result.Should().NotBeNull();
        result.Should().BeOfType<ProfileViewModel>();

        result.Project.Should().BeEquivalentTo(profileExpected.Project);
    }

    [Fact(DisplayName = "Map Profile to ProfileViewModel when Skills are defined")]
    public void MapperProfileEntityToProfileViewModelWithSkills()
    {
        var sequentialGuidGenerator = new SequentialGuidGenerator();
        var clock = new DeterministicClock();
        var skillTypes = SkillSeeder.SeedData.Types.Select(s =>
            {
                var category = SkillCategory.CreateInstance(sequentialGuidGenerator, SkillCategory.DefaultCategoryName);
                return SkillType.CreateInstance(sequentialGuidGenerator, clock, s.Name, category);
            }
        ).ToArray();
        var profile = new ProfileEntityBuilder(_fixture)
            .WithSkillSet()
            .WithSkills(skillTypes)
            .Build();

        var profileExpected = new ProfileViewModel();

        profileExpected = profileExpected with { SkillSet = GetSkillViewModel(profile) };

        var result = profile.ToViewModel();

        result.Should().NotBeNull();
        result.Should().BeOfType<ProfileViewModel>();

        result.SkillSet!.Should().BeEquivalentTo(profileExpected.SkillSet);
    }

    [Fact(DisplayName = "Map Profile to ProfileViewModel when SummaryViewModel is defined")]
    public void MapperProfileEntityToSummaryViewModelWithProject()
    {
        var profile = new ProfileEntityBuilder(_fixture)
            .WithSummary()
            .Build();

        var profileExpected = new ProfileViewModel();

        profileExpected = profileExpected with { Summary = GetSummaryViewModel(profile) };

        var result = profile.ToViewModel();

        result.Should().NotBeNull();
        result.Should().BeOfType<ProfileViewModel>();

        result.Summary.Should().BeEquivalentTo(profileExpected.Summary);
    }

    private static PersonalDataViewModel GetPersonalDataViewModel(Profile entity)
    {
        return new PersonalDataViewModel(
            $"{entity.FirstName} {entity.LastName}",
            entity.Talents.Select(x => x.Description).ToArray(),
            entity.ProfileHeader.ContactInfo.Select(c => new SocialNetworksViewModel(
                c.ContactType.Name,
                c.Value,
                c.ContactType.IconClass ?? string.Empty,
                c.ContactType.IconUrl?.AbsoluteUri ?? string.Empty
            )).ToArray()
        );
    }

    private static AboutViewModel GetAboutViewModel(Profile entity)
    {
        var webSite = GetContactValue("Website");
        var email = GetContactValue("Email");
        var mobilePhone = GetContactValue("Phone");

        return new AboutViewModel(
            entity.ProfileImageUrl,
            entity.ProfileHeader.About,
            entity.ProfileHeader.JobTitle,
            entity.ProfileHeader.Overview,
            entity.BirthDate!.Value,
            webSite,
            mobilePhone,
            entity.ProfileHeader.Address,
            entity.BirthDate.GetAge(),
            entity.ProfileHeader.Title ?? string.Empty,
            email,
            entity.ProfileHeader.Summary ?? string.Empty,
            entity.ProfileHeader.Summary
        );

        string GetContactValue(string contactTypeName)
        {
            return entity.ProfileHeader.ContactInfo
                .FirstOrDefault(x => x.ContactType.Name == contactTypeName)
                ?.Value ?? string.Empty;
        }
    }

    private static TestimonialsViewModel[]? GetTestimonialViewModel(Profile profile)
    {
        return profile.Testimonials.Select(t => new TestimonialsViewModel(
                t.Name,
                t.JobTitle,
                t.PhotoUrl,
                t.Feedback))
            .ToArray();
    }

    private static ProjectViewModel[]? GetProjectViewModel(Profile profile)
    {
        return profile.Portfolio.Select(p => new ProjectViewModel(
                p.Position,
                p.Title,
                p.Description,
                p.UrlLink,
                p.UrlImage,
                p.Type,
                p.UrlVideo))
            .ToArray();
    }

    private static SkillSetViewModel GetSkillViewModel(Profile profile)
    {
        var skills = profile.SkillSet.Skills
            .Select(skill => new SkillDetailsViewModel(skill.SkillType.Name, skill.SkillType.IconUrl.AbsoluteUri, skill.Rank))
            .ToArray();

        return new SkillSetViewModel(profile.SkillSet.Overview!, skills);
    }

    private static SummaryViewModel GetSummaryViewModel(Profile profile)
    {
        var education = profile.CareerHistory.Qualifications
            .Select(e => new AcademicBackgroundViewModel(
                e.Degree,
                e.StartDate.ToString("yyyy-MM-dd"),
                e.InstitutionName,
                e.Description))
            .ToArray();

        var workExperiences = profile.CareerHistory.WorkExperiences
            .Select(w => new PositionViewModel(
                w.JobTitle,
                w.StartDate.ToString("yyyy-MM-dd"),
                w.CompanyLocation,
                w.Description,
                w.Highlights.Select(d => d.Description).ToArray()))
            .ToArray();

        return new SummaryViewModel(
            profile.CareerHistory.Introduction,
            new EducationViewModel(education),
            new WorkExperienceViewModel(workExperiences));
    }

    private static ContactType[] GetContactTypes()
    {
        var sequentialGuidGenerator = new SequentialGuidGenerator();
        var clock = new DeterministicClock();
        var contactTypes = ContactTypeSeeder.SeedData.Types.Select(ct => ContactType.CreateInstance(
            sequentialGuidGenerator,
            clock,
            ct.Name,
            ct.IconClass,
            ct.IconUrl)
        ).ToArray();
        return contactTypes;
    }
}