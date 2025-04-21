using JobMagnet.Infrastructure.Entities;

namespace JobMagnet.Infrastructure.Seeders.Collections;

public record SkillsCollection()
{
    public readonly IReadOnlyList<SkillItemEntity> Skills = new List<SkillItemEntity>
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
}