namespace JobMagnet.Infrastructure.Persistence.Seeders.Collections;

public record SkillsCollection
{
    public static IReadOnlyList<SkillProperties> Data =
    [
        new("HTML", "https://cdn.simpleicons.org/html5", 6, "Software Development", 8),
        new("CSS", "https://cdn.simpleicons.org/css3", 6, "Software Development", 9),
        new("JavaScript", "https://cdn.simpleicons.org/javascript", 7, "Software Development", 2),
        new("C#", "https://cdn.simpleicons.org/dotnet", 9, "Software Development", 1),
        new("TS", "https://cdn.simpleicons.org/typescript", 7, "Software Development", 3),
        new("Angular", "https://cdn.simpleicons.org/angular", 7, "Software Development", 4),
        new("PostgreSQL", "https://cdn.simpleicons.org/postgresql", 6, "Software Development", 6),
        new("React", "https://cdn.simpleicons.org/react", 7, "Software Development", 7),
        new("Bootstrap", "https://cdn.simpleicons.org/bootstrap", 5, "Software Development", 10),
        new("Vue", "https://cdn.simpleicons.org/vuedotjs", 5, "Software Development", 11),
        new("Git", "https://cdn.simpleicons.org/git", 8, "Software Development", 12),
        new("Blazor", "https://cdn.simpleicons.org/blazor", 7, "Software Development", 13),
        new("Rabbit MQ", "https://cdn.simpleicons.org/rabbitmq", 6, "Software Development", 14),
        new("Docker", "https://cdn.simpleicons.org/docker", 8, "Software Development", 15)
    ];

    public record SkillProperties(
        string Name,
        string IconUrl,
        ushort ProficiencyLevel,
        string Category,
        ushort Rank
    );
}