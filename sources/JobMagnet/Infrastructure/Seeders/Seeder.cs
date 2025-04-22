using JobMagnet.Infrastructure.Context;
using JobMagnet.Infrastructure.Entities;
using JobMagnet.Infrastructure.Seeders.Collections;
using ServiceCollection = JobMagnet.Infrastructure.Seeders.Collections.ServiceCollection;

namespace JobMagnet.Infrastructure.Seeders
{
    public class Seeder(JobMagnetDbContext context) : ISeeder
    {
        public async Task RegisterMasterTablesAsync(CancellationToken cancellationToken)
        {
            if (context.ContactTypes.Any()) return;

            await context.ContactTypes.AddRangeAsync(new ContactTypesCollection().GetContactTypes(), cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        }

        public async Task RegisterProfileAsync(CancellationToken cancellationToken)
        {
            var profile = await RegisterProfileDataAsync();
            await context.SaveChangesAsync(cancellationToken);
            var tasks = new List<Task>
            {
                RegisterTalentsAsync(profile.Id),
                RegisterResumeAsync(profile.Id),
                RegisterTestimonialAsync(profile.Id),
                RegisterServiceAsync(profile.Id),
                RegisterSkillAsync(profile.Id),
                RegisterPortfolioAsync(profile.Id)
            };

            await Task.WhenAll(tasks);
            await context.SaveChangesAsync(cancellationToken);
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
            await context.Talents.AddRangeAsync(new TalentsCollection(profileId).Talents);
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

            resumeEntity.ContactInfo = new ContactInfoCollection(resumeEntity.Id).GetContactInfoCollection();

            await context.Resumes.AddAsync(resumeEntity);
        }

        private async Task RegisterSkillAsync(long profileId)
        {
            if (context.Skills.Any()) return;

            var skillEntity = new SkillEntity()
            {
                Id = 0,
                ProfileId = profileId,
                Overview = """
                           I am a passionate web developer with a strong background in front-end and back-end technologies.
                           I have experience in creating dynamic and responsive websites using HTML, CSS, JavaScript, and various frameworks.
                           I am always eager to learn new technologies and improve my skills.
                           """,
                SkillDetails = new SkillsCollection().Skills.ToList(),
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

            FillServiceGalleryItem(serviceEntity);

            await context.Services.AddAsync(serviceEntity);

        }

        private async Task RegisterPortfolioAsync(long profileId)
        {
            if (context.PortfolioGalleries.Any()) return;

            await context.PortfolioGalleries.AddRangeAsync(new PortfolioCollection(profileId).GetPortfolioGallery());
        }

        private async Task RegisterTestimonialAsync(long profileId)
        {
            if (context.Testimonials.Any()) return;

            var testimonials = new TestimonialCollection(profileId).Testimonials;
            await context.Testimonials.AddRangeAsync(testimonials);
        }

        private static void FillServiceGalleryItem(ServiceEntity serviceEntity)
        {
            serviceEntity.GalleryItems = new ServiceCollection(serviceEntity.Id).ServicesGallery.ToList();
        }
    }
}