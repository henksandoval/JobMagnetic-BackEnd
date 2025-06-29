namespace JobMagnet.Shared.Data;

public record RawSkillDefinition(int SkillId, string Name, string Uri, RawSimpleDefinition Category, List<RawSimpleDefinition> Aliases);

public static class SkillRawData
{
    private static readonly RawSimpleDefinition CatSoftwareDev = new(2, "Software Development");

    public static readonly IReadOnlyList<RawSkillDefinition> Data =
    [
        new(101, "HTML", "https://cdn.simpleicons.org/html5", CatSoftwareDev, []),
        new(102, "CSS", "https://cdn.simpleicons.org/css3", CatSoftwareDev, []),
        new(103, "JavaScript", "https://cdn.simpleicons.org/javascript", CatSoftwareDev,
        [
            new RawSimpleDefinition(201, "JS")
        ]),
        new(104, "C#", "https://cdn.simpleicons.org/dotnet", CatSoftwareDev, []),
        new(105, "TypeScript", "https://cdn.simpleicons.org/typescript", CatSoftwareDev,
        [
            new RawSimpleDefinition(202, "TS")
        ]),
        new(106, "Angular", "https://cdn.simpleicons.org/angular", CatSoftwareDev, []),
        new(107, "PostgreSQL", "https://cdn.simpleicons.org/postgresql", CatSoftwareDev,
        [
            new RawSimpleDefinition(203, "Postgres")
        ]),
        new(108, "React", "https://cdn.simpleicons.org/react", CatSoftwareDev, []),
        new(109, "Bootstrap", "https://cdn.simpleicons.org/bootstrap", CatSoftwareDev, []),
        new(110, "Vue", "https://cdn.simpleicons.org/vuedotjs", CatSoftwareDev,
        [
            new RawSimpleDefinition(204, "Vue.js")
        ]),
        new(111, "Git", "https://cdn.simpleicons.org/git", CatSoftwareDev, []),
        new(112, "Blazor", "https://cdn.simpleicons.org/blazor", CatSoftwareDev, []),
        new(113, "RabbitMQ", "https://cdn.simpleicons.org/rabbitmq", CatSoftwareDev,
        [
            new RawSimpleDefinition(205, "Rabbit MQ")
        ]),
        new(114, "Docker", "https://cdn.simpleicons.org/docker", CatSoftwareDev, [])
    ];

    public static int Count => Data.Count;
}