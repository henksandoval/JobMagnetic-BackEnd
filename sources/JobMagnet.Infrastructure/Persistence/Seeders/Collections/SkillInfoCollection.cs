namespace JobMagnet.Infrastructure.Persistence.Seeders.Collections;

public record SkillInfoCollection
{
    public static readonly IReadOnlyList<(string name, ushort proficiencyLevel, ushort rank)> Data =
    [
        ("HTML", 6, 8),
        ("CSS", 6, 9),
        ("JavaScript", 7, 2),
        ("C#", 9, 1),
        ("TS", 7, 3),
        ("Angular", 7, 4),
        ("PostgreSQL", 6, 6),
        ("React", 7, 7),
        ("Bootstrap", 5, 10),
        ("Vue", 5, 11),
        ("Git", 8, 12),
        ("Blazor", 7, 13),
        ("Rabbit MQ", 6, 14),
        ("Docker", 8, 15),
    ];
};