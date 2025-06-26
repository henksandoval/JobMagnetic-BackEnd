using CommunityToolkit.Diagnostics;
using JobMagnet.Domain.Shared.Base;

namespace JobMagnet.Domain.Aggregates.Profiles.Entities;

public readonly record struct TalentId(Guid Value) : IStronglyTypedId<Guid>;

public class Talent : SoftDeletableEntity<TalentId>
{
    public string Description { get; private set; }
    public ProfileId ProfileId { get; private set; }

    private Talent() : base(new TalentId(), Guid.Empty)
    {
    }

    public Talent(string description, ProfileId profileId, TalentId id) : base(id, Guid.Empty)
    {
        Guard.IsNotNullOrWhiteSpace(description);

        Id = id;
        Description = description;
        ProfileId = profileId;
    }
}