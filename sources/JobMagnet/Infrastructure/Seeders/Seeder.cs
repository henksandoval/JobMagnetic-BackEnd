using JobMagnet.Infrastructure.Context;
using JobMagnet.Infrastructure.Entities;

namespace JobMagnet.Infrastructure.Seeders;

public class Seeder(JobMagnetDbContext context) : ISeeder
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

    public async Task RegisterMasterTablesAsync()
    {
        if (context.ContactTypes.Any()) return;

        await context.ContactTypes.AddRangeAsync(ContactTypes);
        await context.SaveChangesAsync();
    }

    public async Task RegisterProfileAsync()
    {
        var profile = await RegisterProfileDataAsync();
        await context.SaveChangesAsync();

        var tasks = new List<Task>
        {
            RegisterTalentsAsync(profile.Id),
            RegisterResumeAsync(profile.Id),
            RegisterTestimonialAsync(profile.Id),
            RegisterServiceAsync(profile.Id),
            RegisterTestimonialAsync(profile.Id),
            RegisterSkillAsync(profile.Id),
            RegisterPortfolioAsync(profile.Id)
        };

        await Task.WhenAll(tasks);

        await context.SaveChangesAsync();
    }

    private async Task<ProfileEntity> RegisterProfileDataAsync()
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

    private async Task RegisterTalentsAsync(long profileId)
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

    private async Task RegisterResumeAsync(long profileId)
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

    private async Task RegisterSkillAsync(long profileId)
    {
        if (context.Skills.Any()) return;

        var skillItemEntities = new List<SkillItemEntity>
        {
            new()
            {
                Id = 0,
                Name = "HTML",
                IconUrl = "https://cdn.simpleicons.org/html5",
                ProficiencyLevel = 6,
                Category = "Software Development",
                SkillId = 0,
                Rank = 8,
                AddedAt = DateTime.Now,
                AddedBy = Guid.Empty
            },
            new()
            {
                Id = 0,
                Name = "CSS",
                IconUrl = "https://cdn.simpleicons.org/css3",
                ProficiencyLevel = 6,
                Category = "Software Development",
                SkillId = 0,
                Rank = 9,
                AddedAt = DateTime.Now,
                AddedBy = Guid.Empty
            },
            new()
            {
                Id = 0,
                Name = "JavaScript",
                IconUrl = "https://cdn.simpleicons.org/javascript",
                ProficiencyLevel = 7,
                Category = "Software Development",
                SkillId = 0,
                Rank = 2,
                AddedAt = DateTime.Now,
                AddedBy = Guid.Empty
            },
            new()
            {
                Id = 0,
                Name = "C#",
                IconUrl = "https://cdn.simpleicons.org/dotnet",
                ProficiencyLevel = 9,
                Category = "Software Development",
                SkillId = 0,
                Rank = 1,
                AddedAt = DateTime.Now,
                AddedBy = Guid.Empty
            },
            new()
            {
                Id = 0,
                Name = "TS",
                IconUrl = "https://cdn.simpleicons.org/typescript",
                ProficiencyLevel = 7,
                Category = "Software Development",
                SkillId = 0,
                Rank = 3,
                AddedAt = DateTime.Now,
                AddedBy = Guid.Empty
            },
            new()
            {
                Id = 0,
                Name = "Angular",
                IconUrl = "https://cdn.simpleicons.org/angular",
                ProficiencyLevel = 7,
                Category = "Software Development",
                SkillId = 0,
                Rank = 4,
                AddedAt = DateTime.Now,
                AddedBy = Guid.Empty
            },
            new()
            {
                Id = 0,
                Name = "PostgreSQL",
                IconUrl = "https://cdn.simpleicons.org/postgresql",
                ProficiencyLevel = 6,
                Category = "Software Development",
                SkillId = 0,
                Rank = 6,
                AddedAt = DateTime.Now,
                AddedBy = Guid.Empty
            },
            new()
            {
                Id = 0,
                Name = "React",
                IconUrl = "https://cdn.simpleicons.org/react",
                ProficiencyLevel = 7,
                Category = "Software Development",
                SkillId = 0,
                Rank = 7,
                AddedAt = DateTime.Now,
                AddedBy = Guid.Empty
            },
            new()
            {
                Id = 0,
                Name = "Bootstrap",
                IconUrl = "https://cdn.simpleicons.org/bootstrap",
                ProficiencyLevel = 5,
                Category = "Software Development",
                SkillId = 0,
                Rank = 10,
                AddedAt = DateTime.Now,
                AddedBy = Guid.Empty
            },
            new()
            {
                Id = 0,
                Name = "Vue",
                IconUrl = "https://cdn.simpleicons.org/vuedotjs",
                ProficiencyLevel = 5,
                Category = "Software Development",
                SkillId = 0,
                Rank = 11,
                AddedAt = DateTime.Now,
                AddedBy = Guid.Empty
            },
            new()
            {
                Id = 0,
                Name = "Git",
                IconUrl = "https://cdn.simpleicons.org/git",
                ProficiencyLevel = 8,
                Category = "Software Development",
                SkillId = 0,
                Rank = 12,
                AddedAt = DateTime.Now,
                AddedBy = Guid.Empty
            },
            new()
            {
                Id = 0,
                Name = "Blazor",
                IconUrl = "https://cdn.simpleicons.org/blazor",
                ProficiencyLevel = 7,
                Category = "Software Development",
                SkillId = 0,
                Rank = 13,
                AddedAt = DateTime.Now,
                AddedBy = Guid.Empty
            },
            new()
            {
                Id = 0,
                Name = "Rabbit MQ",
                IconUrl = "https://cdn.simpleicons.org/rabbitmq",
                ProficiencyLevel = 6,
                Category = "Software Development",
                SkillId = 0,
                Rank = 14,
                AddedAt = DateTime.Now,
                AddedBy = Guid.Empty
            },
            new()
            {
                Id = 0,
                Name = "Docker",
                IconUrl = "https://cdn.simpleicons.org/docker",
                ProficiencyLevel = 8,
                Category = "Software Development",
                SkillId = 0,
                Rank = 15,
                AddedAt = DateTime.Now,
                AddedBy = Guid.Empty
            }
        };

        var skillEntity = new SkillEntity()
        {
            Id = 0,
            ProfileId = profileId,
            Overview = """
                       I am a passionate web developer with a strong background in front-end and back-end technologies.
                       I have experience in creating dynamic and responsive websites using HTML, CSS, JavaScript, and various frameworks.
                       I am always eager to learn new technologies and improve my skills.
                       """,
            SkillDetails = skillItemEntities,
            AddedAt = DateTime.Now,
            AddedBy = Guid.Empty
        };

        await context.Skills.AddAsync(skillEntity);
    }

    private async Task RegisterServiceAsync(long profileId)
    {
        if (context.Services.Any()) return;

        var serviceEntity = new ServiceEntity
        {
            Id = 0,
            Overview = "I offer a wide range of web development services, including front-end and back-end development, UI/UX design, and more.",
            ProfileId = profileId,
            AddedAt = DateTime.Now,
            AddedBy = Guid.Empty
        };

        ServiceGalleryItem(serviceEntity);

        await context.Services.AddAsync(serviceEntity);

    }

    private async Task RegisterPortfolioAsync(long profileId)
    {
        if (context.PortfolioGalleries.Any()) return;

        var portfolioEntities = new PortfolioGalleryEntity[]
        {
            new()
            {
                Id = 0,
                Position = 1,
                Title = "Aventuras Animales",
                Description = "Cada fotografía captura momentos únicos y comportamientos fascinantes.",
                UrlLink = "https://waylet.es/",
                UrlImage = "https://images.pexels.com/photos/617278/pexels-photo-617278.jpeg",
                Type = "CAT",
                UrlVideo = string.Empty,
                ProfileId = profileId,
                AddedAt = DateTime.Now,
                AddedBy = Guid.Empty
            },
            new()
            {
                Id = 0,
                Position = 2,
                Title = "Horizontes Naturales",
                Description =
                    "Cada imagen captura la esencia de lugares únicos, desde montañas imponentes hasta costas tranquilas, invitándote a explorar la belleza del mundo",
                UrlLink = "https://biati-digital.github.io/glightbox/",
                UrlImage = "https://th.bing.com/th/id/OIP.iwFhHHKPOqAJUDO-iSov_wHaE8?rs=1&pid=ImgDetMain",
                Type = "NATURE",
                UrlVideo = string.Empty,
                ProfileId = profileId,
                AddedAt = DateTime.Now,
                AddedBy = Guid.Empty
            },
            new()
            {
                Id = 0,
                Position = 3,
                Title = "Movil Truck",
                Description =
                    "Plataforma de transporte inteligente; solución tecnológica diseñada para abordar de manera eficiente el transporte de mercancías por carretera.",
                UrlLink = "https://moviltruck.com/",
                UrlImage = "https://moviltruck.com/wp-content/uploads/2023/11/Hero-1-.png",
                Type = "WebPage",
                UrlVideo = string.Empty,
                ProfileId = profileId,
                AddedAt = DateTime.Now,
                AddedBy = Guid.Empty
            },
            new()
            {
                Id = 0,
                Position = 4,
                Title = "Aventuras Animales",
                Description = "Cada fotografía captura momentos únicos y comportamientos fascinantes.",
                UrlLink = string.Empty,
                UrlImage = "https://images.pexels.com/photos/617278/pexels-photo-617278.jpeg",
                Type = "CAT",
                UrlVideo = string.Empty,
                ProfileId = profileId,
                AddedAt = DateTime.Now,
                AddedBy = Guid.Empty
            },
            new()
            {
                Id = 0,
                Position = 5,
                Title = "Aventuras Animales",
                Description = "Cada fotografía captura momentos únicos y comportamientos fascinantes.",
                UrlLink = string.Empty,
                UrlImage = "https://images.pexels.com/photos/617278/pexels-photo-617278.jpeg",
                Type = "CAT",
                UrlVideo = string.Empty,
                ProfileId = profileId,
                AddedAt = DateTime.Now,
                AddedBy = Guid.Empty
            },
            new()
            {
                Id = 0,
                Position = 6,
                Title = "Music",
                Description =
                    "Cada imagen captura la esencia de la musica, el sonido llega al alma dando una hermosa sensacion de relajacion",
                UrlLink = "",
                UrlImage = "https://i0.wp.com/www.nus.agency/wp-content/uploads/2023/03/musica-arte-scaled.jpg?ssl=1",
                Type = "Music",
                UrlVideo = string.Empty,
                ProfileId = profileId,
                AddedAt = DateTime.Now,
                AddedBy = Guid.Empty
            },
            new()
            {
                Id = 0,
                Position = 7,
                Title = "Red And Blue Parrot",
                Description = "Hermosos y encantadores Guacamayas en ambiente natural.",
                UrlLink = string.Empty,
                UrlImage =
                    "https://images.pexels.com/photos/1427447/pexels-photo-1427447.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1",
                Type = "NATURE",
                UrlVideo = "https://videos.pexels.com/video-files/17325162/17325162-uhd_1440_2560_30fps.mp4",
                ProfileId = profileId,
                AddedAt = DateTime.Now,
                AddedBy = Guid.Empty
            }
        };

        await context.PortfolioGalleries.AddRangeAsync(portfolioEntities);
    }

    private async Task RegisterTestimonialAsync(long profileId)
    {
        if (context.Testimonials.Any()) return;

        var testimonialEntities = new TestimonialEntity[]
        {
            new()
            {
                Id = 0,
                Name = "Jane Smith",
                JobTitle = "Project Manager",
                PhotoUrl = "https://randomuser.me/api/portraits/women/28.jpg",
                Feedback =
                    "Brandon is a talented developer who consistently delivers high-quality work. His ability to understand client needs and translate them into functional designs is impressive.",
                ProfileId = profileId,
                AddedAt = DateTime.Now,
                AddedBy = Guid.Empty
            },
            new()
            {
                Id = 0,
                Name = "Alice Johnson",
                JobTitle = "Software Engineer",
                PhotoUrl = "https://randomuser.me/api/portraits/women/82.jpg",
                Feedback =
                    "Working with Brandon has been a pleasure. He is always willing to go the extra mile to ensure the project is a success. His technical skills and creativity are top-notch.",
                ProfileId = profileId,
                AddedAt = DateTime.Now,
                AddedBy = Guid.Empty
            },
            new()
            {
                Id = 0,
                Name = "John Smith",
                JobTitle = "UX Designer",
                PhotoUrl = "https://randomuser.me/api/portraits/men/31.jpg",
                Feedback = "The project was delivered on time and exceeded our expectations. Highly recommend!",
                ProfileId = profileId,
                AddedAt = DateTime.Now,
                AddedBy = Guid.Empty
            },
            new()
            {
                Id = 0,
                Name = "Michael Brown",
                JobTitle = "CTO",
                PhotoUrl = "https://randomuser.me/api/portraits/men/82.jpg",
                Feedback =
                    "The team consistently delivered beyond expectations and maintained excellent communication.",
                ProfileId = profileId,
                AddedAt = DateTime.Now,
                AddedBy = Guid.Empty
            },
            new()
            {
                Id = 0,
                Name = "Emily Davis",
                JobTitle = "Product Owner",
                PhotoUrl = "https://randomuser.me/api/portraits/women/11.jpg",
                Feedback =
                    "Their innovative solutions and commitment to quality have been pivotal in our project’s success, making them an invaluable partner in our journey.",
                ProfileId = profileId,
                AddedAt = DateTime.Now,
                AddedBy = Guid.Empty
            },
        };
        await context.Testimonials.AddRangeAsync(testimonialEntities);
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
                ContactTypeId = 12,
                ResumeId = resumeEntity.Id,
                AddedAt = DateTime.Now,
                AddedBy = Guid.Empty
            },
            new()
            {
                Id = 0,
                Value = "https://linkedin.com/in/brandonjohnson",
                ContactTypeId = 6,
                ResumeId = resumeEntity.Id,
                AddedAt = DateTime.Now,
                AddedBy = Guid.Empty
            },
            new()
            {
                Id = 0,
                Value = "https://github.com/brandonjohnson",
                ContactTypeId = 7,
                ResumeId = resumeEntity.Id,
                AddedAt = DateTime.Now,
                AddedBy = Guid.Empty
            },
            new()
            {
                Id = 0,
                Value = "https://twitter.com/brandonjohnson",
                ContactTypeId = 8,
                ResumeId = resumeEntity.Id,
                AddedAt = DateTime.Now,
                AddedBy = Guid.Empty
            },
            new()
            {
                Id = 0,
                Value = "https://brandonjohnson.dev",
                ContactTypeId = 5,
                ResumeId = resumeEntity.Id,
                AddedAt = DateTime.Now,
                AddedBy = Guid.Empty
            },
            new()
            {
                Id = 0,
                Value = "https://instagram.com/brandonjohnson",
                ContactTypeId = 10,
                ResumeId = resumeEntity.Id,
                AddedAt = DateTime.Now,
                AddedBy = Guid.Empty
            },
            new()
            {
                Id = 0,
                Value = "https://facebook.com/brandonjohnson",
                ContactTypeId = 9,
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
            }
        };
    }

    private static void ServiceGalleryItem(ServiceEntity serviceEntity)
     {
         serviceEntity.GalleryItems = new List<ServiceGalleryItemEntity>
         {
             new()
             {
                 Id = 0,
                 Position = 1,
                 Title = "Web Development",
                 Description = "Building responsive and user-friendly websites.",
                 UrlLink = "https://example.com/web-development",
                 UrlImage = "https://cdn.pixabay.com/photo/2024/08/06/10/43/wine-8949009_1280.jpg",
                 UrlVideo = "https://example.com/video1.mp4",
                 Type = "image",
                 ServiceId = serviceEntity.Id,
                 AddedAt = DateTime.Now,
                 AddedBy = Guid.Empty
             },
             new()
             {
                 Id = 0,
                 Position = 2,
                 Title = "UI/UX Design",
                 Description = "Creating intuitive and engaging user interfaces.",
                 UrlLink = "https://example.com/ui-ux-design",
                 UrlImage = "https://cdn.pixabay.com/photo/2023/08/11/08/29/highland-cattle-8183107_1280.jpg",
                 UrlVideo = "https://example.com/video2.mp4",
                 Type = "image",
                 ServiceId = serviceEntity.Id,
                 AddedAt = DateTime.Now,
                 AddedBy = Guid.Empty
             },
             new()
             {
                 Id = 0,
                 Position = 3,
                 Title = "Web and Brand Graphic Design",
                 Description = "Creating intuitive and engaging user interfaces.",
                 UrlLink = "https://example.com/ux-design",
                 UrlImage = "https://cdn.pixabay.com/photo/2024/02/20/13/21/mountains-8585535_1280.jpg",
                 UrlVideo = "https://example.com/video3.mp4",
                 Type = "image",
                 ServiceId = serviceEntity.Id,
                 AddedAt = DateTime.Now,
                 AddedBy = Guid.Empty
             },
             new()
             {
                 Id = 0,
                 Position = 4,
                 Title = "SEO Consulting",
                 Description = "Creating intuitive and engaging user interfaces.",
                 UrlLink = "https://example.com/ui-ux-design2",
                 UrlImage = "https://cdn.pixabay.com/photo/2024/01/25/10/50/mosque-8531576_1280.jpg",
                 UrlVideo = "https://example.com/video4.mp4",
                 Type = "image",
                 ServiceId = serviceEntity.Id,
                 AddedAt = DateTime.Now,
                 AddedBy = Guid.Empty
             }
         };
     }
}