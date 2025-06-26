using System.Collections.Immutable;
using JobMagnet.Domain.Aggregates.Profiles.Entities;
using JobMagnet.Domain.Core.Entities;

namespace JobMagnet.Infrastructure.Persistence.Seeders.Collections;

// ReSharper disable once NotAccessedPositionalProperty.Global
public record Talents(string Description);

public record TalentsCollection
{
    private readonly long _profileId;

    private readonly IList<Talents> _values =
    [
        new ("Strategic Thinker"),
        new ("Inspirational Leader"),
        new ("Team Player"),
        new ("Proactive"),
        new ("Resilient")
    ];

    public TalentsCollection(long profileId = 0)
    {
        _profileId = profileId;
    }

    public ImmutableList<Talent> GetTalents()
    {
        return _values.Select(talent => new Talent
        (
            talent.Description,
            _profileId
        )).ToImmutableList();
    }
}