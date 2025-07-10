using System.Collections.Immutable;
using JobMagnet.Domain.Aggregates.Profiles.Entities;


namespace JobMagnet.Infrastructure.Persistence.Seeders.Collections;

public record Talents(string Description);

public record TalentsSeeder
{
    private readonly IList<Talents> _values =
    [
        new ("Strategic Thinker"),
        new ("Inspirational Leader"),
        new ("Team Player"),
        new ("Proactive"),
        new ("Resilient")
    ];

    public ImmutableList<Talent> GetTalents()
    {
        return _values.Select(talent => Talent.CreateInstance(talent.Description)).ToImmutableList();
    }
}