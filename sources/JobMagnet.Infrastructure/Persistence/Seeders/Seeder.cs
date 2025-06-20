using JobMagnet.Domain.Core.Entities;
using JobMagnet.Infrastructure.Exceptions;
using JobMagnet.Infrastructure.Persistence.Context;
using JobMagnet.Infrastructure.Persistence.Seeders.Collections;
using Microsoft.EntityFrameworkCore;

namespace JobMagnet.Infrastructure.Persistence.Seeders;

public interface ISeeder
{
    Task RegisterMasterTablesAsync(CancellationToken cancellationToken);
    Task RegisterProfileAsync(CancellationToken cancellationToken);
}

public class Seeder(JobMagnetDbContext context) : ISeeder
{
    public async Task RegisterMasterTablesAsync(CancellationToken cancellationToken)
    {
        if (context.ContactTypes.Any()) return;

        var contactTypesWithAliases = new ContactTypesCollection().GetContactTypesWithAliases();
        var skillsWithAliases = new SkillTypesCollection().GetSkillTypesWithAliases();

        await context.ContactTypes.AddRangeAsync(contactTypesWithAliases, cancellationToken);
        await context.SkillTypes.AddRangeAsync(skillsWithAliases, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

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
        AddServices(profile);
        AddPortfolio(profile);
        AddTestimonials(profile);

        return profile;
    }

    private static void AddPublicIdentifier(ProfileEntity profile)
    {
        var publicProfile = new PublicProfileIdentifierEntity(profile, "john-doe-1a2b3c");
        profile.PublicProfileIdentifiers.Add(publicProfile);
    }

    private static void AddTalents(ProfileEntity profile)
    {
        List<string> talentsCollection = ["Creative", "Problem Solver", "Team Player", "Fast Learner"];
        foreach (var talent in talentsCollection)
        {
            profile.AddTalent(talent);
        }
    }

    private async Task AddResumeAsync(ProfileEntity profile, CancellationToken cancellationToken)
    {
        var contactTypeMap = await BuildContactTypesMapAsync(cancellationToken).ConfigureAwait(false);

        var resume = new ResumeEntity
        {
            Id = 0,
            JobTitle = "UI/UX Designer & Web Developer",
            Title = "Mr.",
            About = """
                    ¡Hello! I'm Johnson Brandon, a passionate web developer who loves creating dynamic, easy-to-use websites.
                    I have over 5 years of experience in the technology industry, working with a variety of clients to make their visions a reality.
                    """,
            Summary = """
                      Developed and maintained web applications for various clients, focusing on front-end development and user experience.
                      Assisted in the development of websites and applications, learning best practices and improving coding skills.",
                      """,
            Overview = """
                       In my free time I enjoy hiking, reading science fiction novels, and experimenting with new technologies.
                       I am always eager to learn new things and take on exciting challenges.",
                       """,
            Address = "123 Main St, Springfield, USA",
            AddedAt = DateTime.Now,
            AddedBy = Guid.Empty,
        };

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
        var skillSet = new SkillSetEntity(overview, profile.Id);

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
        var summary = new SummaryEntity
        {
            Id = 0,
            Introduction =
                "Professional with experience in your area or profession, recognized for key skills. Committed to value or professional goal, seeking to contribute to the growth of company or industry.",
            Education = new SummaryCollection().GetEducation().ToList(),
            WorkExperiences = new SummaryCollection().GetWorkExperience().ToList()
        };
        profile.AddSummary(summary);
    }

    private static void AddServices(ProfileEntity profile)
    {
        var service = new ServiceEntity
        {
            Id = 0,
            Overview =
                "I offer a wide range of web development services, including front-end and back-end development, UI/UX design, and more.",
            GalleryItems = new ServiceCollection().GetServicesGallery().ToList()
        };
        profile.AddService(service);
    }

    private static void AddPortfolio(ProfileEntity profile)
    {
        var portfolioItems = new PortfolioCollection().GetPortfolioGallery();
        foreach (var item in portfolioItems) profile.AddPortfolioItem(item);
    }

    private static void AddTestimonials(ProfileEntity profile)
    {
        var testimonials = new TestimonialCollection().GetTestimonials();
        foreach (var testimonial in testimonials) profile.AddTestimonial(testimonial);
    }

    private async Task<Dictionary<string, ContactTypeEntity>> BuildContactTypesMapAsync(
        CancellationToken cancellationToken)
    {
        var map = new Dictionary<string, ContactTypeEntity>(StringComparer.OrdinalIgnoreCase);

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