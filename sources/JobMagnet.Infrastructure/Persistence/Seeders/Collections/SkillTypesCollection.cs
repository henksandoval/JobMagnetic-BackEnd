using System.Collections.Immutable;
using JobMagnet.Domain.Core.Entities;

namespace JobMagnet.Infrastructure.Persistence.Seeders.Collections;

public record SkillTypesCollection
{
    private readonly IReadOnlyList<(string type, string uri, string category, string[] aliases)> _values =
    [
        new("HTML", "https://cdn.simpleicons.org/html5", "Software Development", []),
        new("CSS", "https://cdn.simpleicons.org/css3", "Software Development", []),
        new("JavaScript", "https://cdn.simpleicons.org/javascript", "Software Development", []),
        new("C#", "https://cdn.simpleicons.org/dotnet", "Software Development", []),
        new("TS", "https://cdn.simpleicons.org/typescript", "Software Development", []),
        new("Angular", "https://cdn.simpleicons.org/angular", "Software Development", []),
        new("PostgreSQL", "https://cdn.simpleicons.org/postgresql", "Software Development", []),
        new("React", "https://cdn.simpleicons.org/react", "Software Development", []),
        new("Bootstrap", "https://cdn.simpleicons.org/bootstrap", "Software Development", []),
        new("Vue", "https://cdn.simpleicons.org/vuedotjs", "Software Development", []),
        new("Git", "https://cdn.simpleicons.org/git", "Software Development", []),
        new("Blazor", "https://cdn.simpleicons.org/blazor", "Software Development", []),
        new("Rabbit MQ", "https://cdn.simpleicons.org/rabbitmq", "Software Development", []),
        new("Docker", "https://cdn.simpleicons.org/docker", "Software Development", []),
    ];

    public ImmutableList<SkillType> GetSkillTypesWithAliases()
    {
        var skills = new List<SkillType>();

        foreach (var item in _values)
        {
            var category = new SkillCategory(item.category);
            var skill = new SkillType(0, item.type, category, new Uri(item.uri));

            foreach (var alias in item.aliases)
            {
                skill.AddAlias(alias);
            }

            skills.Add(skill);
        }

        return skills.ToImmutableList();
    }
}