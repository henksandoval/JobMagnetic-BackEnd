using System.Collections.Immutable;
using JobMagnet.Infrastructure.Entities;

namespace JobMagnet.Infrastructure.Seeders.Collections;

// ReSharper disable once NotAccessedPositionalProperty.Global
public record TalentsCollection
{
    private readonly string[] _values =
    {
        "Creative",
        "Problem Solver",
        "Team Player",
        "Fast Learner"
    };

    private readonly long _profileId;

    public TalentsCollection(long profileId = 0)
    {
        _profileId = profileId;
    }

    public ImmutableList<TalentEntity> GetTalents()
    {
        return _values.Select(talent => new TalentEntity
        {
            Id = 0,
            Description = talent,
            ProfileId = _profileId,
            AddedAt = DateTime.Now,
            AddedBy = Guid.Empty
        }).ToImmutableList();
    }
}