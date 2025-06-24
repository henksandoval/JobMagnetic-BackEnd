using System.Collections.Immutable;
using JobMagnet.Domain.Core.Entities;

namespace JobMagnet.Infrastructure.Persistence.Seeders.Collections;

// ReSharper disable once NotAccessedPositionalProperty.Global
public record Talents(string Description, long ProfileId, ProfileEntity Profile);
public record TalentsCollection
{
    private readonly long _profileId;

    private readonly IList<Talents> _values =
    [
        new ("Creative", 1, new ProfileEntity { Id = 1 }),
        new ("Problem Solver", 1, new ProfileEntity { Id = 1 }),
        new ("Detail Oriented", 1, new ProfileEntity { Id = 1 }),
        new ("Excellent Communicator", 1, new ProfileEntity { Id = 1 }),
        new ("Adaptable", 1, new ProfileEntity { Id = 1 }),
        new ("Analytical Thinker", 1, new ProfileEntity { Id = 1 }),
        new ("Innovative", 1, new ProfileEntity { Id = 1 }),
        new ("Self-Motivated", 1, new ProfileEntity { Id = 1 }),
        new ("Collaborative", 1, new ProfileEntity { Id = 1 }),
        new ("Time Management", 1, new ProfileEntity { Id = 1 }),
        new ("Critical Thinking", 1, new ProfileEntity { Id = 1 }),
        new ("Leadership Skills", 1, new ProfileEntity { Id = 1 }),
        new ("Technical Proficiency", 1, new ProfileEntity { Id = 1 }),
        new ("Project Management", 1, new ProfileEntity { Id = 1 }),
        new ("Customer Focused", 1, new ProfileEntity { Id = 1 }),
        new ("Team Player", 1, new ProfileEntity { Id = 1 }),
        new ("Fast Learner", 1, new ProfileEntity { Id = 1 })
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
                talent.ProfileId, 
                talent.Profile,
                _profileId
            )).ToImmutableList();
    }
}