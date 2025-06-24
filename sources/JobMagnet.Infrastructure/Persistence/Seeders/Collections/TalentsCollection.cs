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
        new ("Creative"),
        new ("Problem Solver"),
        new ("Detail Oriented"),
        new ("Excellent Communicator"),
        new ("Adaptable"),
        new ("Analytical Thinker"),
        new ("Innovative"),
        new ("Self-Motivated"),
        new ("Collaborative"),
        new ("Time Management"),
        new ("Critical Thinking"),
        new ("Leadership Skills"),
        new ("Technical Proficiency"),
        new ("Project Management"),
        new ("Customer Focused"),
        new ("Team Player"),
        new ("Fast Learner"),
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