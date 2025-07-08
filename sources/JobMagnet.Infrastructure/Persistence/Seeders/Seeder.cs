using CSharpFunctionalExtensions;
using JobMagnet.Domain.Aggregates.Contact;
using JobMagnet.Domain.Aggregates.Profiles;
using JobMagnet.Domain.Aggregates.Profiles.Entities;
using JobMagnet.Domain.Aggregates.Profiles.ValueObjects;
using JobMagnet.Domain.Aggregates.SkillTypes;
using JobMagnet.Infrastructure.Exceptions;
using JobMagnet.Infrastructure.Persistence.Context;
using JobMagnet.Infrastructure.Persistence.Seeders.Collections;
using JobMagnet.Shared.Abstractions;
using JobMagnet.Shared.Data;
using Microsoft.EntityFrameworkCore;

namespace JobMagnet.Infrastructure.Persistence.Seeders;

public interface ISeeder
{
    Task<Maybe<ProfileId>> RegisterProfileAsync(CancellationToken cancellationToken);
}

public class Seeder(JobMagnetDbContext context, IGuidGenerator guidGenerator, IClock clock) : ISeeder
{
    private const string? About = """
                                  ¡Hello! I'm Johnson Brandon, a passionate web developer who loves creating dynamic, easy-to-use websites.
                                  I have over 5 years of experience in the technology industry, working with a variety of clients to make their visions a reality.
                                  """;

    private const string? Summary = """
                                    Developed and maintained web applications for various clients, focusing on front-end development and user experience.
                                    Assisted in the development of websites and applications, learning best practices and improving coding skills.",
                                    """;

    private const string? Overview = """
                                     In my free time I enjoy hiking, reading science fiction novels, and experimenting with new technologies.
                                     I am always eager to learn new things and take on exciting challenges.",
                                     """;
    private static readonly IList<(string Name, string JobTitle, string PhotoUrl, string Feedback)> Testimonials =
    [
        ("Jane Smith", "Portfolio Manager", "https://randomuser.me/api/portraits/women/28.jpg",
            "Brandon is a talented developer who consistently delivers high-quality work. His ability to understand client needs and translate them into functional designs is impressive."),
        ("Alice Johnson", "Software Engineer", "https://randomuser.me/api/portraits/women/82.jpg",
            "Working with Brandon has been a pleasure. He is always willing to go the extra mile to ensure the project is a success. His technical skills and creativity are top-notch."),
        ("John Smith", "UX Designer", "https://randomuser.me/api/portraits/men/31.jpg",
            "The project was delivered on time and exceeded our expectations. Highly recommend!"),
        ("Michael Brown", "CTO", "https://randomuser.me/api/portraits/men/82.jpg",
            "The team consistently delivered beyond expectations and maintained excellent communication."),
        ("Emily Davis", "Product Owner", "https://randomuser.me/api/portraits/women/11.jpg",
            "Their innovative solutions and commitment to quality have been pivotal in our project’s success, making them an invaluable partner in our journey.")
    ];

    public static int TestimonialsCount => Testimonials.Count;

    public async Task<Maybe<ProfileId>> RegisterProfileAsync(CancellationToken cancellationToken)
    {
        if (await context.Profiles.AnyAsync(p => p.FirstName == "John" && p.LastName == "Doe", cancellationToken))
            return Maybe<ProfileId>.None;

        var sampleProfile = await BuildSampleProfileAsync(cancellationToken);

        await context.Profiles.AddAsync(sampleProfile, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return sampleProfile.Id;
    }

    private async Task<Profile> BuildSampleProfileAsync(CancellationToken cancellationToken)
    {
        var profile = Profile.CreateInstance(
            guidGenerator,
            clock,
            "John",
            "Doe",
            "https://bootstrapmade.com/content/demo/MyResume/assets/img/profile-img.jpg",
            new DateOnly(1990, 04, 01));

        AddPublicIdentifier(profile);
        AddTalents(profile);
        await AddProfileHeaderAsync(profile, cancellationToken).ConfigureAwait(false);
        await AddSkills(profile, cancellationToken).ConfigureAwait(false);
        AddCareerHistory(profile);
        AddProject(profile);
        AddTestimonials(profile);

        return profile;
    }

    private void AddPublicIdentifier(Profile profile)
    {
        profile.AddVanityUrl(guidGenerator,"john-doe-1a2b3c");
    }

    private static void AddTalents(Profile profile)
    {
        var talentsCollection = new TalentsSeeder().GetTalents();
        foreach (var talent in talentsCollection)
            profile.AddTalent(talent.Description);
    }

    private async Task AddProfileHeaderAsync(Profile profile, CancellationToken cancellationToken)
    {
        var contactTypeMap = await BuildContactTypesMapAsync(cancellationToken).ConfigureAwait(false);

        var resume = ProfileHeader.CreateInstance(
            guidGenerator,
            profile.Id,
            "Mr.",
            "",
            "UI/UX Designer & Web Developer",
            About ?? null,
            Summary ?? null,
            Overview ?? null,
            "123 Main St, Springfield, USA");

        foreach (var (value, contactTypeName) in ContactInfoRawData.Data)
            if (contactTypeMap.TryGetValue(contactTypeName, out var contactType))
                resume.AddContactInfo(guidGenerator, clock, value, contactType);
            else
                throw new JobMagnetInfrastructureException(
                    $"Seeding error: Contact type '{contactTypeName}' not found in database.");

        profile.AddResume(resume);
    }

    private async Task AddSkills(Profile profile, CancellationToken cancellationToken)
    {
        var skillTypeMap = await BuildSkillTypesMapAsync(cancellationToken).ConfigureAwait(false);

        const string overview = """
                                I am a passionate web developer with a strong background in front-end and back-end technologies.
                                I have experience in creating dynamic and responsive websites using HTML, CSS, JavaScript, and various frameworks.
                                I am always eager to learn new technologies and improve my skills.
                                """;
        var skillSet = SkillSet.CreateInstance(guidGenerator, profile.Id, overview);

        foreach (var (skillName, proficiencyLevel, rank) in SkillInfoCollection.Data)
            if (skillTypeMap.TryGetValue(skillName, out var skillType))
                skillSet.AddSkill(guidGenerator, proficiencyLevel, skillType);
            else
                throw new JobMagnetInfrastructureException(
                    $"Seeding error: Skill type '{skillName}' not found in database.");

        profile.AddSkillSet(skillSet);
    }

    private void AddCareerHistory(Profile profile)
    {
        var careerHistory = CareerHistory.CreateInstance(
            guidGenerator,
            profile.Id,
            "Professional with experience in your area or profession, recognized for key skills. Committed to value or professional goal, seeking to contribute to the growth of company or industry.");

        var careerHistorySeeder = new CareerHistorySeeder(guidGenerator, clock, careerHistory.Id);

        foreach (var education in careerHistorySeeder.GetQualifications().ToList())
            careerHistory.AddEducation(education);

        foreach (var workExperience in careerHistorySeeder.GetWorkExperience().ToList())
            careerHistory.AddWorkExperience(workExperience);

        profile.AddSummary(careerHistory);
    }

    private void AddProject(Profile profile)
    {
        foreach (var item in ProjectRawData.Data)
        {
            _ = profile.AddProject(
                guidGenerator,
                item.Title,
                item.Description,
                item.UrlLink,
                item.UrlImage,
                item.UrlVideo,
                item.Type
            );
        }
    }

    private void AddTestimonials(Profile profile)
    {
        foreach (var item in Testimonials)
            profile.AddTestimonial(
                guidGenerator,
                item.Name,
                item.JobTitle,
                item.Feedback,
                item.PhotoUrl);
    }

    private async Task<Dictionary<string, ContactType>> BuildContactTypesMapAsync(
        CancellationToken cancellationToken)
    {
        var map = new Dictionary<string, ContactType>(StringComparer.OrdinalIgnoreCase);

        var allTypes = await context.ContactTypes
            .Include(ct => ct.Aliases)
            .ToListAsync(cancellationToken);

        foreach (var type in allTypes)
        {
            map[type.Name] = type;
            foreach (var alias in type.Aliases) map[alias.Alias] = type;
        }

        return map;
    }

    private async Task<Dictionary<string, SkillType>> BuildSkillTypesMapAsync(
        CancellationToken cancellationToken)
    {
        var map = new Dictionary<string, SkillType>(StringComparer.OrdinalIgnoreCase);

        var allTypes = await context.SkillTypes
            .Include(ct => ct.Aliases)
            .AsTracking()
            .ToListAsync(cancellationToken);

        foreach (var type in allTypes)
        {
            map[type.Name] = type;
            foreach (var alias in type.Aliases) map[alias.Alias] = type;
        }

        return map;
    }
}