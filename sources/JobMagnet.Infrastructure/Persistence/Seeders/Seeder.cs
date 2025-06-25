using JobMagnet.Domain.Core.Entities;
using JobMagnet.Domain.Core.Entities.Contact;
using JobMagnet.Domain.Core.Entities.Skills;
using JobMagnet.Domain.Core.Enums;
using JobMagnet.Infrastructure.Exceptions;
using JobMagnet.Infrastructure.Persistence.Context;
using JobMagnet.Infrastructure.Persistence.Seeders.Collections;
using Microsoft.EntityFrameworkCore;

namespace JobMagnet.Infrastructure.Persistence.Seeders;

public interface ISeeder
{
    Task RegisterProfileAsync(CancellationToken cancellationToken);
}

public class Seeder(JobMagnetDbContext context) : ISeeder
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

    public async Task RegisterProfileAsync(CancellationToken cancellationToken)
    {
        if (await context.Profiles.AnyAsync(p => p.FirstName == "John" && p.LastName == "Doe", cancellationToken))
            return;

        var sampleProfile = await BuildSampleProfileAsync(cancellationToken);

        await context.Profiles.AddAsync(sampleProfile, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    private async Task<ProfileEntity> BuildSampleProfileAsync(CancellationToken cancellationToken)
    {
        var profile = new ProfileEntity
        {
            Id = 0,
            FirstName = "John",
            LastName = "Doe",
            BirthDate = new DateOnly(1990, 04, 01),
            ProfileImageUrl = "https://bootstrapmade.com/content/demo/MyResume/assets/img/profile-img.jpg",
            AddedAt = DateTime.Now,
            AddedBy = Guid.Empty
        };

        AddPublicIdentifier(profile);
        AddTalents(profile);
        await AddResumeAsync(profile, cancellationToken).ConfigureAwait(false);
        await AddSkills(profile, cancellationToken).ConfigureAwait(false);
        AddSummary(profile);
        AddProject(profile);
        AddTestimonials(profile);

        return profile;
    }

    private static void AddPublicIdentifier(ProfileEntity profile)
    {
        profile.VanityUrls.AddPublicProfileIdentifier("john-doe-1a2b3c");
    }

    private static void AddTalents(ProfileEntity profile)
    {
        var talentsCollection = new TalentsCollection().GetTalents();
        foreach (var talent in talentsCollection)
        {
            profile.AddTalent(talent.Description);
        }
    }

    private async Task AddResumeAsync(ProfileEntity profile, CancellationToken cancellationToken)
    {
        var contactTypeMap = await BuildContactTypesMapAsync(cancellationToken).ConfigureAwait(false);

        var resume = new ResumeEntity("Mr.",
            "",
            "UI/UX Designer & Web Developer",
            About,
            Summary,
            Overview,
            "123 Main St, Springfield, USA");

        foreach (var (value, contactTypeName) in ContactInfoCollection.Data)
        {
            if (contactTypeMap.TryGetValue(contactTypeName, out var contactType))
            {
                resume.AddContactInfo(value, contactType);
            }
            else
            {
                throw new JobMagnetInfrastructureException(
                    $"Seeding error: Contact type '{contactTypeName}' not found in database.");
            }
        }

        profile.AddResume(resume);
    }

    private async Task AddSkills(ProfileEntity profile, CancellationToken cancellationToken)
    {
        var skillTypeMap = await BuildSkillTypesMapAsync(cancellationToken).ConfigureAwait(false);

        const string overview = """
                                I am a passionate web developer with a strong background in front-end and back-end technologies.
                                I have experience in creating dynamic and responsive websites using HTML, CSS, JavaScript, and various frameworks.
                                I am always eager to learn new technologies and improve my skills.
                                """;
        var skillSet = new SkillSet(overview, profile.Id);

        foreach (var (skillName, proficiencyLevel, rank) in SkillInfoCollection.Data)
        {
            if (skillTypeMap.TryGetValue(skillName, out var skillType))
            {
                skillSet.AddSkill(proficiencyLevel, skillType);
            }
            else
            {
                throw new JobMagnetInfrastructureException(
                    $"Seeding error: Skill type '{skillName}' not found in database.");
            }
        }

        profile.AddSkill(skillSet);
    }

    private static void AddSummary(ProfileEntity profile)
    {
        var summary = new SummaryEntity(
            "Professional with experience in your area or profession, recognized for key skills. Committed to value or professional goal, seeking to contribute to the growth of company or industry.",
            profile.Id);

        foreach (var education in new SummaryCollection().GetEducation().ToList())
            summary.AddEducation(education);

        foreach (var workExperience in new SummaryCollection().GetWorkExperience().ToList())
            summary.AddWorkExperience(workExperience);

        profile.AddSummary(summary);
    }

    private static void AddProject(ProfileEntity profile)
    {
        var ProjectItems = new ProjectCollection().GetProjects();
        foreach (var item in ProjectItems) profile.AddProjectToPortfolio(item);
    }

    private static void AddTestimonials(ProfileEntity profile)
    {
        var testimonials = new TestimonialCollection().GetTestimonials();
        foreach (var item in testimonials)
            profile.SocialProof.AddTestimonial(item.Name, item.JobTitle, item.Feedback, item.PhotoUrl);
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
            foreach (var alias in type.Aliases)
            {
                map[alias.Alias] = type;
            }
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
            foreach (var alias in type.Aliases)
            {
                map[alias.Alias] = type;
            }
        }

        return map;
    }
}