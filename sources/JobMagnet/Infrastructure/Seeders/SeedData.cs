using JobMagnet.Infrastructure.Context;
using JobMagnet.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace JobMagnet.Infrastructure.Seeders;

public static class SeedData
{
    public static async Task InitializeAsync(IServiceProvider serviceProvider)
    {
        await using var context = serviceProvider.GetRequiredService<JobMagnetDbContext>();

        await context.Database.MigrateAsync();

        var profile = await RegisterProfileDataAsync(context);
        await RegisterTalentsAsync(context, profile.Id);

        await context.SaveChangesAsync();
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
}