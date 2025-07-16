using AutoFixture;
using Bogus;
using JobMagnet.Domain.Aggregates.Contact;
using JobMagnet.Domain.Aggregates.Profiles;
using JobMagnet.Domain.Aggregates.Profiles.Entities;
using JobMagnet.Domain.Aggregates.Profiles.ValueObjects;
using JobMagnet.Domain.Aggregates.SkillTypes;
using JobMagnet.Infrastructure.Persistence.Context;
using JobMagnet.Shared.Abstractions;
using JobMagnet.Shared.Tests.Abstractions;
using JobMagnet.Shared.Tests.Fixtures.Customizations;
// ReSharper disable ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract

namespace JobMagnet.Shared.Tests.Fixtures.Builders;

public class ProfileEntityBuilder
{
    private static readonly Faker Faker = FixtureBuilder.Faker;
    private readonly List<(string value, ContactType contactType)> _contactInfo = [];
    private readonly IFixture _fixture;
    private readonly IGuidGenerator _guidGenerator;
    private CareerHistory _careerHistory = null!;
    private SkillSet _skillSet = null!;
    private ProfileHeader _profileHeader = null!;
    private List<Project> _projects = [];
    private List<Talent> _talents = [];
    private List<Testimonial> _testimonials = [];
    private PersonName _personName;

    public ProfileEntityBuilder(IFixture fixture, JobMagnetDbContext context = null!)
    {
        _fixture = fixture;
        _guidGenerator = new SequentialGuidGenerator();
    }

    public ProfileEntityBuilder WithHeader(bool loadProfileHeader = true)
    {
        if (loadProfileHeader)
            _profileHeader = _fixture.Create<ProfileHeader>();

        return this;
    }

    public ProfileEntityBuilder WithContactInfo(ContactType[] availableContactTypes, int count = 5)
    {
        if (count == 0)
            return this;

        if (count > availableContactTypes.Length)
            throw new ArgumentOutOfRangeException(nameof(count),
                $"Count exceeds the number of available contact types. ({availableContactTypes.Length})");

        if (_profileHeader == null)
            throw new InvalidOperationException("Cannot add contact info without a profileHeader. Call WithProfileHeader() first.");

        while (_contactInfo.Count < count)
        {
            var contactType = Faker.PickRandom(availableContactTypes);
            if (_contactInfo.Any(ci => ci.contactType == contactType)) continue;

            var value = GenerateContactDetails(contactType.Name);
            _contactInfo.Add((value, contactType));
        }

        return this;
    }

    public ProfileEntityBuilder WithSkillSet(bool loadSkill = true)
    {
        if (loadSkill)
            _skillSet = _fixture.Create<SkillSet>();

        return this;
    }

    public ProfileEntityBuilder WithSkills(SkillType[] availableSkillTypes, int count = 5)
    {
        if (count == 0)
            return this;

        if (count > availableSkillTypes.Length)
            throw new ArgumentOutOfRangeException(nameof(count),
                $"Count exceeds the number of available skill types. ({availableSkillTypes.Length})");

        if (_skillSet == null) throw new InvalidOperationException("Cannot add skills without a skill set. Call WithSkillSet() first.");

        while (_skillSet.Skills.Count < count)
        {
            var skillsToAdd = Faker.PickRandom(availableSkillTypes);
            if (_skillSet.SkillExists(skillsToAdd)) continue;

            var proficiencyLevel = (ushort)Faker.Random.Int(1, 10);
            _skillSet.AddSkill(_guidGenerator, proficiencyLevel, skillsToAdd);
        }

        return this;
    }

    public ProfileEntityBuilder WithTalents(int count = 5)
    {
        if (count >= StaticCustomizations.Talents.Length)
            throw new ArgumentOutOfRangeException(nameof(count), "Count exceeds the number of available talents.");

        var talents = new List<Talent>();

        while (talents.Count < count)
        {
            var talent = _fixture.Create<Talent>();

            if (talents.Any(t => t.Description == talent.Description))
                continue;

            talents.Add(talent);
        }

        _talents = talents;

        return this;
    }

    public ProfileEntityBuilder WithProjects(int count = 5)
    {
        _projects = _fixture.CreateMany<Project>(count).ToList();
        return this;
    }

    public ProfileEntityBuilder WithCareerHistory(bool loadCareerHistory = true)
    {
        if (loadCareerHistory)
            _careerHistory = _fixture.Create<CareerHistory>();

        return this;
    }

    public ProfileEntityBuilder WithEducation(int count = 5)
    {
        if (_careerHistory == null && count > 0)
            throw new InvalidOperationException("Cannot add education without a careerHistory. Call WithCareerHistory() first.");

        foreach (var education in _fixture.CreateMany<AcademicDegree>(count).ToList())
            _careerHistory!.AddEducation(
                _guidGenerator,
                education.Degree,
                education.InstitutionName,
                education.InstitutionLocation,
                education.StartDate,
                education.EndDate,
                education.Description);

        return this;
    }

    public ProfileEntityBuilder WithWorkExperience(int count = 5)
    {
        if (_careerHistory == null && count > 0)
            throw new InvalidOperationException("Cannot add work experience without a careerHistory. Call WithCareerHistory() first.");

        foreach (var workExperience in _fixture.CreateMany<WorkExperience>(count).ToList())
            _careerHistory!.AddWorkExperience(
                _guidGenerator,
                workExperience.JobTitle,
                workExperience.CompanyName,
                workExperience.CompanyLocation,
                workExperience.StartDate,
                workExperience.EndDate,
                workExperience.Description);

        return this;
    }

    public ProfileEntityBuilder WithTestimonials(int count = 5)
    {
        _testimonials = _fixture.CreateMany<Testimonial>(count).ToList();
        return this;
    }

    public ProfileEntityBuilder WithName(string? firstName, string? lastName)
    {
        _personName = new PersonName(firstName, lastName, applyValidations: false);

        return this;
    }

    public Profile Build()
    {
        var profile = BuildBasicProfile();

        LoadHeader(profile);
        LoadContactInfo(profile);
        LoadTestimonials(profile);
        LoadProjects(profile);
        LoadCareerHistory(profile);
        LoadSkillSet(profile);
        LoadTalents(profile);

        return profile;
    }

    private Profile BuildBasicProfile()
    {
        var profile = _fixture.Create<Profile>();

        if (_personName is not null)
            profile.ChangeName(_personName, new DeterministicClock());

        return profile;
    }

    private void LoadHeader(Profile profile)
    {
        if (_profileHeader is null)
            return;

        profile.AddHeader(
            _guidGenerator,
            _profileHeader.Title,
            _profileHeader.Suffix,
            _profileHeader.JobTitle,
            _profileHeader.About,
            _profileHeader.Summary,
            _profileHeader.Overview,
            _profileHeader.Address
        );
    }

    private void LoadContactInfo(Profile profile)
    {
        if (_contactInfo.Count <= 0) return;

        foreach (var contactInfo in _contactInfo)
            profile.AddContactInfo(_guidGenerator, contactInfo.value, contactInfo.contactType);
    }

    private void LoadSkillSet(Profile profile)
    {
        if (_skillSet is null)
            return;

        profile.AddSkillSet(_skillSet);
    }

    private void LoadCareerHistory(Profile profile)
    {
        if (_careerHistory is null)
            return;

        profile.AddCareerHistory(_careerHistory);
    }

    private void LoadTestimonials(Profile profile)
    {
        if (_testimonials.Count <= 0) return;

        foreach (var item in _testimonials)
            profile.AddTestimonial(
                _guidGenerator,
                item.Name,
                item.JobTitle,
                item.Feedback,
                item.PhotoUrl);
    }

    private void LoadProjects(Profile profile)
    {
        if (_projects.Count <= 0) return;

        foreach (var gallery in _projects)
            _ = profile.AddProject(
                _guidGenerator,
                gallery.Title,
                gallery.Description,
                gallery.UrlLink,
                gallery.UrlImage,
                gallery.UrlVideo,
                gallery.Type);
    }

    private void LoadTalents(Profile profile)
    {
        if (_talents.Count <= 0) return;

        foreach (var talent in _talents)
            profile.AddTalent(
                _guidGenerator,
                talent.Description
                );
    }

    private static string GenerateContactDetails(string contactType)
    {
        return contactType switch
        {
            "Email" => Faker.Person.Email,
            "Phone" => Faker.Phone.PhoneNumber(),
            "LinkedIn" => $"https://linkedin.com/in/{Faker.Internet.UserName()}",
            "GitHub" => $"https://github.com/{Faker.Internet.UserName()}",
            "Website" => Faker.Internet.Url(),
            _ => Faker.Internet.DomainName()
        };
    }
}