using System.Collections.Immutable;
using JobMagnet.Domain.Core.Entities;

namespace JobMagnet.Infrastructure.Persistence.Seeders.Collections;

// ReSharper disable once NotAccessedPositionalProperty.Global
public record Talents(string Description);

public record TalentsCollection
{
    private readonly long _profileId;

    private readonly IList<Talents> _values =
    [
        new("Creative"),
        new("Problem Solver"),
        new("Detail Oriented"),
        new("Excellent Communicator"),
        new("Adaptable")
    ];

    public TalentsCollection(long profileId = 0)
    {
        _profileId = profileId;
    }

    public ImmutableList<TalentEntity> GetTalents()
    {
        return _values.Select(talent => new TalentEntity
        (
            talent.Description,
            _profileId
        )).ToImmutableList();
    }
}