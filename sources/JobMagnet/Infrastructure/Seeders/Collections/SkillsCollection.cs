using System.Collections.Immutable;
using JobMagnet.Infrastructure.Entities;

namespace JobMagnet.Infrastructure.Seeders.Collections;

public record SkillsCollection()
{
    private record SkillProperties(
        string Name,
        string IconUrl,
        ushort ProficiencyLevel,
        string Category,
        long SkillId,
        ushort Rank
    );

    private readonly IList<SkillProperties> _values = new List<SkillProperties>
    {
        new("HTML", "https://cdn.simpleicons.org/html5", 6, "Software Development", 0, 8),
        new("CSS", "https://cdn.simpleicons.org/css3", 6, "Software Development", 0, 9),
        new("JavaScript", "https://cdn.simpleicons.org/javascript", 7, "Software Development", 0, 2),
        new("C#", "https://cdn.simpleicons.org/dotnet", 9, "Software Development", 0, 1),
        new("TS", "https://cdn.simpleicons.org/typescript", 7, "Software Development", 0, 3),
        new("Angular", "https://cdn.simpleicons.org/angular", 7, "Software Development", 0, 4),
        new("PostgreSQL", "https://cdn.simpleicons.org/postgresql", 6, "Software Development", 0, 6),
        new("React", "https://cdn.simpleicons.org/react", 7, "Software Development", 0, 7),
        new("Bootstrap", "https://cdn.simpleicons.org/bootstrap", 5, "Software Development", 0, 10),
        new("Vue", "https://cdn.simpleicons.org/vuedotjs", 5, "Software Development", 0, 11),
        new("Git", "https://cdn.simpleicons.org/git", 8, "Software Development", 0, 12),
        new("Blazor", "https://cdn.simpleicons.org/blazor", 7, "Software Development", 0, 13),
        new("Rabbit MQ", "https://cdn.simpleicons.org/rabbitmq", 6, "Software Development", 0, 14),
        new("Docker", "https://cdn.simpleicons.org/docker", 8, "Software Development", 0, 15)
    };

    public ImmutableList<SkillItemEntity> GetSkills()
    {
        return _values.Select(x => new SkillItemEntity
        {
            Id = 0,
            Name = x.Name,
            IconUrl = x.IconUrl,
            ProficiencyLevel = x.ProficiencyLevel,
            Category = x.Category,
            SkillId = x.SkillId,
            Rank = x.Rank,
            AddedAt = DateTime.Now,
            AddedBy = Guid.Empty
        }).ToImmutableList();
    }
}