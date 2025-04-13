using JobMagnet.Infrastructure.Context;
using JobMagnet.Infrastructure.Entities;

namespace JobMagnet.Infrastructure.Seeders;

public static class SeedData
{
    public static async Task InitializeAsync(IServiceProvider serviceProvider)
    {
        await using var context = serviceProvider.GetRequiredService<JobMagnetDbContext>();

        await context.Database.EnsureCreatedAsync();

        RegisterProfileData(context);

        await context.SaveChangesAsync();
    }

    private static void RegisterProfileData(JobMagnetDbContext context)
    {
        if (context.Profile.Any()) return;

        context.Profile.AddRange(
            new ProfileEntity
            {
                Id = 0,
                FirstName = "John",
                LastName = "Doe",
                BirthDate = new DateOnly(1990, 04, 01),
                ProfileImageUrl = "https://bootstrapmade.com/content/demo/MyResume/assets/img/profile-img.jpg",
                AddedAt = DateTime.Now,
                AddedBy = Guid.Empty
            }
        );
    }
}