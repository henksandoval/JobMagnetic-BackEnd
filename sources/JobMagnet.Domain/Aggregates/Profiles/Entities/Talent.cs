using System.Diagnostics.CodeAnalysis;
using CommunityToolkit.Diagnostics;
using JobMagnet.Domain.Core.Entities.Base;

namespace JobMagnet.Domain.Aggregates.Profiles.Entities;

public class Talent : SoftDeletableEntity<long>
{
    public string Description { get; private set; }
    public long ProfileId { get; private set; }

    private Talent()
    {
    }

    [SetsRequiredMembers]
    public Talent(string description, long profileId = 0, long id = 0)
    {
        Guard.IsGreaterThanOrEqualTo(id, 0);
        Guard.IsGreaterThanOrEqualTo(profileId, 0);
        Guard.IsNotNullOrWhiteSpace(description);

        Id = id;
        Description = description;
        ProfileId = profileId;
    }
}