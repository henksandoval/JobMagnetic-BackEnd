using AutoFixture;
using Bogus;
using JobMagnet.Domain.Aggregates.Contact;
using JobMagnet.Domain.Aggregates.Profiles;
using JobMagnet.Domain.Aggregates.Profiles.Entities;
using JobMagnet.Domain.Aggregates.Profiles.ValueObjects;
using JobMagnet.Domain.Aggregates.SkillTypes;
using JobMagnet.Shared.Abstractions;
using JobMagnet.Shared.Tests.Abstractions;
using JobMagnet.Shared.Tests.Fixtures.Customizations;
using JobMagnet.Shared.Utils;

namespace JobMagnet.Shared.Tests.Fixtures.Builders;

public class ProfileEntityBuilder
{
    private static readonly Faker Faker = FixtureBuilder.Faker;
    private readonly IFixture _fixture;
    private readonly IGuidGenerator _guidGenerator;
    private readonly Profile _profile;
    private bool _keepNameSet = false;

    public ProfileEntityBuilder(IFixture fixture)
    {
        _fixture = fixture;
        _guidGenerator = new SequentialGuidGenerator();
        _profile = Profile.CreateEmptyInstance(_guidGenerator, new DeterministicClock());
    }

    public ProfileEntityBuilder WithHeader(bool loadProfileHeader = true)
    {
        if (loadProfileHeader.IsFalse())
            return this;

        var header = _fixture.Create<ProfileHeader>();
        _profile.AddHeader(
            _guidGenerator,
            header.Title,
            header.Suffix,
            header.JobTitle,
            header.About,
            header.Summary,
            header.Overview,
            header.Address
        );

        return this;
    }

    public ProfileEntityBuilder WithContactInfo(ContactType[] availableContactTypes, int count = 5)
    {
        if (count == 0)
            return this;

        if (count > availableContactTypes.Length)
            throw new ArgumentOutOfRangeException(nameof(count),
                $"Count exceeds the number of available contact types. ({availableContactTypes.Length})");

        if (_profile.HaveHeader.IsFalse())
            throw new InvalidOperationException("Cannot add contact info without a profileHeader. Call WithProfileHeader() first.");

        var selectedTypes = new HashSet<ContactType>();
        while (selectedTypes.Count < count)
        {
            var contactType = Faker.PickRandom(availableContactTypes);
            if (!selectedTypes.Add(contactType)) continue;

            var value = GenerateContactDetails(contactType.Name);
            _profile.AddContactInfo(_guidGenerator, value, contactType);
        }

        return this;
    }

    public ProfileEntityBuilder WithSkillSet(bool loadSkill = true)
    {
        if (loadSkill.IsFalse())
            return this;

        var skillSet = _fixture.Create<SkillSet>();
        _profile.AddSkillSet(skillSet);

        return this;
    }

    public ProfileEntityBuilder WithSkills(SkillType[] availableSkillTypes, int count = 5)
    {
        if (count == 0)
            return this;

        if (count > availableSkillTypes.Length)
            throw new ArgumentOutOfRangeException(nameof(count),
                $"Count exceeds the number of available skill types. ({availableSkillTypes.Length})");

        if (_profile.HaveSkillSet.IsFalse())
            throw new InvalidOperationException("Cannot add skills without a skill set. Call WithSkillSet() first.");

        while (_profile.SkillSet!.Skills.Count < count)
        {
            var skillType = Faker.PickRandom(availableSkillTypes);
            if (_profile.SkillExists(skillType)) continue;

            var proficiency = (ushort)Faker.Random.Int(1, 10);
            _profile.AddSkill(_guidGenerator, proficiency, skillType);
        }

        return this;
    }

    public ProfileEntityBuilder WithTalents(int count = 5)
    {
        if (count >= StaticCustomizations.Talents.Length)
            throw new ArgumentOutOfRangeException(nameof(count), "Count exceeds the number of available talents.");

        while (_profile.TalentShowcase.Count < count)
        {
            var talent = _fixture.Create<Talent>();
            if (_profile.TalentExist(talent.Description)) continue;
            _profile.AddTalent(_guidGenerator, talent.Description);
        }

        return this;
    }

    public ProfileEntityBuilder WithProjects(int count = 5)
    {
        foreach (var project in _fixture.CreateMany<Project>(count))
        {
            _profile.AddProject(
                _guidGenerator,
                project.Title,
                project.Description,
                project.UrlLink,
                project.UrlImage,
                project.UrlVideo,
                project.Type);
        }

        return this;
    }

    public ProfileEntityBuilder WithCareerHistory(bool loadCareerHistory = true)
    {
        if (loadCareerHistory.IsFalse())
            return this;

        var careerHistory = _fixture.Create<CareerHistory>();
        _profile.AddCareerHistory(careerHistory);

        return this;
    }

    public ProfileEntityBuilder WithEducation(int count = 5)
    {
        if (_profile.HaveCareerHistory.IsFalse() && count > 0)
            throw new InvalidOperationException("Cannot add education without a careerHistory. Call WithCareerHistory() first.");

        while (_profile.AcademicDegreesInCareerHistory.Count < count)
        {
            var degree = _fixture.Create<AcademicDegree>();
            if (_profile.AcademicDegreeExistsInCareerHistory(degree.Degree, degree.InstitutionName)) continue;

            _profile.AddAcademicDegreeToCareerHistory(
                _guidGenerator,
                degree.Degree,
                degree.InstitutionName,
                degree.InstitutionLocation,
                degree.StartDate,
                degree.EndDate,
                degree.Description);
        }

        return this;
    }

    public ProfileEntityBuilder WithWorkExperience(int count = 5)
    {
        if (_profile.HaveCareerHistory.IsFalse() && count > 0)
            throw new InvalidOperationException("Cannot add work experience without a careerHistory. Call WithCareerHistory() first.");

        while (_profile.WorkExperiencesInCareerHistory.Count < count)
        {
            var workExperience = _fixture.Create<WorkExperience>();
            if (_profile.WorkExperienceExists(workExperience.JobTitle, workExperience.CompanyName, workExperience.StartDate))
                continue;

            _profile.AddWorkExperienceToCareerHistory(
                _guidGenerator,
                workExperience.JobTitle,
                workExperience.CompanyName,
                workExperience.CompanyLocation,
                workExperience.StartDate,
                workExperience.EndDate,
                workExperience.Description);
        }

        return this;
    }

    public ProfileEntityBuilder WithTestimonials(int count = 5)
    {
        foreach (var testimonial in _fixture.CreateMany<Testimonial>(count))
        {
            _profile.AddTestimonial(
                _guidGenerator,
                testimonial.Name,
                testimonial.JobTitle,
                testimonial.Feedback,
                testimonial.PhotoUrl);
        }

        return this;
    }

    public ProfileEntityBuilder WithName(string? firstName, string? lastName)
    {
        var name = new PersonName(firstName, lastName, applyValidations: false);
        _profile.ChangeName(name, new DeterministicClock());
        _keepNameSet = true;
        return this;
    }

    public Profile Build()
    {
        var tempProfile = _fixture.Create<Profile>();

        if (_keepNameSet.IsFalse())
            _profile.ChangeName(tempProfile.Name, new DeterministicClock());

        _profile.ChangeBirthDate(tempProfile.BirthDate, new DeterministicClock());
        _profile.ChangeProfileImage(tempProfile.ProfileImage, new DeterministicClock());

        return _profile;
    }

    private static string GenerateContactDetails(string contactType) =>
        contactType switch
        {
            "Email" => Faker.Person.Email,
            "Phone" => Faker.Phone.PhoneNumber(),
            "LinkedIn" => $"https://linkedin.com/in/{Faker.Internet.UserName()}",
            "GitHub" => $"https://github.com/{Faker.Internet.UserName()}",
            "Website" => Faker.Internet.Url(),
            _ => Faker.Internet.DomainName()
        };
}