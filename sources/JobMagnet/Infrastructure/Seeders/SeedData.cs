using JobMagnet.Infrastructure.Context;
using JobMagnet.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace JobMagnet.Infrastructure.Seeders;

public static class SeedData
{
    public static readonly List<ContactTypeEntity> ContactTypes =
    [
        new()
        {
            Id = 0,
            Name = "Email",
            IconClass = "bx bx-envelope",
            AddedAt = DateTime.Now,
            AddedBy = Guid.Empty
        },
        new()
        {
            Id = 0,
            Name = "Mobile Phone",
            IconClass = "bx bx-mobile",
            AddedAt = DateTime.Now,
            AddedBy = Guid.Empty
        },
        new()
        {
            Id = 0,
            Name = "Home Phone",
            IconClass = "bx bx-phone",
            AddedAt = DateTime.Now,
            AddedBy = Guid.Empty
        },
        new()
        {
            Id = 0,
            Name = "Work Phone",
            IconClass = "bx bx-phone-call",
            AddedAt = DateTime.Now,
            AddedBy = Guid.Empty
        },
        new()
        {
            Id = 0,
            Name = "Website",
            IconClass = "bx bx-globe",
            AddedAt = DateTime.Now,
            AddedBy = Guid.Empty
        },
        new()
        {
            Id = 0,
            Name = "LinkedIn",
            IconClass = "bx bxl-linkedin",
            AddedAt = DateTime.Now,
            AddedBy = Guid.Empty
        },
        new()
        {
            Id = 0,
            Name = "GitHub",
            IconClass = "bx bxl-github",
            AddedAt = DateTime.Now,
            AddedBy = Guid.Empty
        },
        new()
        {
            Id = 0,
            Name = "Twitter",
            IconClass = "bx bxl-twitter",
            AddedAt = DateTime.Now,
            AddedBy = Guid.Empty
        },
        new()
        {
            Id = 0,
            Name = "Facebook",
            IconClass = "bx bxl-facebook",
            AddedAt = DateTime.Now,
            AddedBy = Guid.Empty
        },
        new()
        {
            Id = 0,
            Name = "Instagram",
            IconClass = "bx bxl-instagram",
            AddedAt = DateTime.Now,
            AddedBy = Guid.Empty
        },
        new()
        {
            Id = 0,
            Name = "YouTube",
            IconClass = "bx bxl-youtube",
            AddedAt = DateTime.Now,
            AddedBy = Guid.Empty
        },
        new()
        {
            Id = 0,
            Name = "WhatsApp",
            IconClass = "bx bxl-whatsapp",
            AddedAt = DateTime.Now,
            AddedBy = Guid.Empty
        },
        new()
        {
            Id = 0,
            Name = "Telegram",
            IconClass = "bx bxl-telegram",
            AddedAt = DateTime.Now,
            AddedBy = Guid.Empty
        },
        new()
        {
            Id = 0,
            Name = "Snapchat",
            IconClass = "bx bxl-snapchat",
            AddedAt = DateTime.Now,
            AddedBy = Guid.Empty
        },
        new()
        {
            Id = 0,
            Name = "Pinterest",
            IconClass = "bx bxl-pinterest",
            AddedAt = DateTime.Now,
            AddedBy = Guid.Empty
        },
        new()
        {
            Id = 0,
            Name = "Skype",
            IconClass = "bx bxl-skype",
            AddedAt = DateTime.Now,
            AddedBy = Guid.Empty
        },
        new()
        {
            Id = 0,
            Name = "Discord",
            IconClass = "bx bxl-discord",
            AddedAt = DateTime.Now,
            AddedBy = Guid.Empty
        },
        new()
        {
            Id = 0,
            Name = "Twitch",
            IconClass = "bx bxl-twitch",
            AddedAt = DateTime.Now,
            AddedBy = Guid.Empty
        },
        new()
        {
            Id = 0,
            Name = "TikTok",
            IconClass = "bx bxl-tiktok",
            AddedAt = DateTime.Now,
            AddedBy = Guid.Empty
        },
        new()
        {
            Id = 0,
            Name = "Reddit",
            IconClass = "bx bxl-reddit",
            AddedAt = DateTime.Now,
            AddedBy = Guid.Empty
        },
        new()
        {
            Id = 0,
            Name = "Vimeo",
            IconClass = "bx bxl-vimeo",
            AddedAt = DateTime.Now,
            AddedBy = Guid.Empty
        }
    ];

    public static async Task InitializeAsync(IServiceProvider serviceProvider)
    {
        await using var context = serviceProvider.GetRequiredService<JobMagnetDbContext>();

        await context.Database.MigrateAsync();

        await RegisterMasterTablesAsync(context);
        var profile = await RegisterProfileDataAsync(context);
        var registerTasks  = new List<Task>
        {
            RegisterTalentsAsync(context, profile.Id),
            RegisterResumeAsync(context, profile.Id),
        };

        await Task.WhenAll(registerTasks);

        await context.SaveChangesAsync();
    }

    private static async Task RegisterMasterTablesAsync(JobMagnetDbContext context)
    {
        if (context.ContactTypes.Any()) return;

        await context.ContactTypes.AddRangeAsync(ContactTypes);
    }

    private static async Task<ProfileEntity> RegisterProfileDataAsync(JobMagnetDbContext context)
    {
        if (context.Profiles.Any()) return context.Profiles.FirstOrDefault()!;

        var profileEntity = new ProfileEntity
        {
            Id = 0,
            FirstName = "John",
            LastName = "Doe",
            BirthDate = new DateOnly(1990, 04, 01),
            ProfileImageUrl = "https://bootstrapmade.com/content/demo/MyResume/assets/img/profile-img.jpg",
            AddedAt = DateTime.Now,
            AddedBy = Guid.Empty
        };

        await context.Profiles.AddAsync(profileEntity);
        return profileEntity;
    }

    private static async Task RegisterTalentsAsync(JobMagnetDbContext context, long profileId)
    {
        if (context.Talents.Any()) return;

        var talents = new List<TalentEntity>
        {
            new()
            {
                Id = 0,
                Description = "Creative",
                ProfileId = profileId,
                AddedAt = DateTime.Now,
                AddedBy = Guid.Empty
            },
            new()
            {
                Id = 0,
                Description = "Problem Solver",
                ProfileId = profileId,
                AddedAt = DateTime.Now,
                AddedBy = Guid.Empty
            },
            new()
            {
                Id = 0,
                Description = "Team Player",
                ProfileId = profileId,
                AddedAt = DateTime.Now,
                AddedBy = Guid.Empty
            },
            new()
            {
                Id = 0,
                Description = "Fast Learner",
                ProfileId = profileId,
                AddedAt = DateTime.Now,
                AddedBy = Guid.Empty
            },
        };

        await context.Talents.AddRangeAsync(talents);
    }

    private static async Task RegisterResumeAsync(JobMagnetDbContext context, long profileId)
    {
        if (context.Resumes.Any()) return;

        var resumeEntity = new ResumeEntity
        {
            Id = 0,
            ProfileId = profileId,
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
            AddedBy = Guid.Empty
        };

        FillContactInfo(resumeEntity);

        await context.Resumes.AddAsync(resumeEntity);
    }

    private static void FillContactInfo(ResumeEntity resumeEntity)
    {
        resumeEntity.ContactInfo = new List<ContactInfoEntity>
        {
            new()
            {
                Id = 0,
                Value = "brandon.johnson@example.com",
                ContactTypeId = 1,
                ResumeId = resumeEntity.Id,
                AddedAt = DateTime.Now,
                AddedBy = Guid.Empty
            },
            new()
            {
                Id = 0,
                Value = "+1234567890",
                ContactTypeId = 2,
                ResumeId = resumeEntity.Id,
                AddedAt = DateTime.Now,
                AddedBy = Guid.Empty
            },
            new()
            {
                Id = 0,
                Value = "https://linkedin.com/in/brandonjohnson",
                ContactTypeId = 3,
                ResumeId = resumeEntity.Id,
                AddedAt = DateTime.Now,
                AddedBy = Guid.Empty
            },
            new()
            {
                Id = 0,
                Value = "https://github.com/brandonjohnson",
                ContactTypeId = 4,
                ResumeId = resumeEntity.Id,
                AddedAt = DateTime.Now,
                AddedBy = Guid.Empty
            },
            new()
            {
                Id = 0,
                Value = "https://twitter.com/brandonjohnson",
                ContactTypeId = 5,
                ResumeId = resumeEntity.Id,
                AddedAt = DateTime.Now,
                AddedBy = Guid.Empty
            },
            new()
            {
                Id = 0,
                Value = "https://brandonjohnson.dev",
                ContactTypeId = 6,
                ResumeId = resumeEntity.Id,
                AddedAt = DateTime.Now,
                AddedBy = Guid.Empty
            },
            new()
            {
                Id = 0,
                Value = "https://instagram.com/brandonjohnson",
                ContactTypeId = 7,
                ResumeId = resumeEntity.Id,
                AddedAt = DateTime.Now,
                AddedBy = Guid.Empty
            },
            new()
            {
                Id = 0,
                Value = "https://facebook.com/brandonjohnson",
                ContactTypeId = 8,
                ResumeId = resumeEntity.Id,
                AddedAt = DateTime.Now,
                AddedBy = Guid.Empty
            },
            new()
            {
                Id = 0,
                Value = "+1234567890",
                ContactTypeId = 9,
                ResumeId = resumeEntity.Id,
                AddedAt = DateTime.Now,
                AddedBy = Guid.Empty
            }
        };
    }
}