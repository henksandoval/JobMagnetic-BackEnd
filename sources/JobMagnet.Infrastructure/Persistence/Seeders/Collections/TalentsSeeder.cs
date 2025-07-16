using System.Collections.Immutable;
using JobMagnet.Domain.Aggregates.Profiles.Entities;
using JobMagnet.Domain.Aggregates.Profiles.ValueObjects;
using JobMagnet.Shared.Abstractions;

namespace JobMagnet.Infrastructure.Persistence.Seeders.Collections;

public record Talents(string Description);

public record TalentsSeeder
{
    private readonly IGuidGenerator _guidGenerator;
    private readonly ProfileId _profileId;
    
    private readonly IList<Talents> _values =
    [
        new ("Strategic Thinker"),
        new ("Inspirational Leader"),
        new ("Team Player"),
        new ("Proactive"),
        new ("Resilient")
    ];
    
    public TalentsSeeder(IGuidGenerator guidGenerator, ProfileId profileId)
    {
        _guidGenerator = guidGenerator;
        _profileId = profileId;
    }

    public ImmutableList<Talent> GetTalents()
    {
        return _values.Select(talent => Talent.CreateInstance(_guidGenerator, _profileId, talent.Description)).ToImmutableList();
    }
}