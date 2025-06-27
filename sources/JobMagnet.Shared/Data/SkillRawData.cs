namespace JobMagnet.Shared.Data;

public record RawSkillDefinition(string Name, string Uri, string CategoryName, string[] Aliases);

public static class SkillRawData
{
    public static readonly IReadOnlyList<RawSkillDefinition> Data =
    [
        new("HTML", "https://cdn.simpleicons.org/html5", "Software Development", []),
        new("CSS", "https://cdn.simpleicons.org/css3", "Software Development", []),
        new("JavaScript", "https://cdn.simpleicons.org/javascript", "Software Development", ["JS"]),
        new("C#", "https://cdn.simpleicons.org/dotnet", "Software Development", []),
        new("TypeScript", "https://cdn.simpleicons.org/typescript", "Software Development", ["TS"]),
        new("Angular", "https://cdn.simpleicons.org/angular", "Software Development", []),
        new("PostgreSQL", "https://cdn.simpleicons.org/postgresql", "Software Development", ["Postgres"]),
        new("React", "https://cdn.simpleicons.org/react", "Software Development", []),
        new("Bootstrap", "https://cdn.simpleicons.org/bootstrap", "Software Development", []),
        new("Vue", "https://cdn.simpleicons.org/vuedotjs", "Software Development", ["Vue.js"]),
        new("Git", "https://cdn.simpleicons.org/git", "Software Development", []),
        new("Blazor", "https://cdn.simpleicons.org/blazor", "Software Development", []),
        new("RabbitMQ", "https://cdn.simpleicons.org/rabbitmq", "Software Development", ["Rabbit MQ"]),
        new("Docker", "https://cdn.simpleicons.org/docker", "Software Development", [])
    ];

    public static int Count => Data.Count;
}