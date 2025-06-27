using CommunityToolkit.Diagnostics;
using JobMagnet.Domain.Shared.Base;
using JobMagnet.Domain.Shared.Base.Interfaces;
using JobMagnet.Shared.Abstractions;

namespace JobMagnet.Domain.Aggregates.Profiles.Entities;

public readonly record struct TalentId(Guid Value) : IStronglyTypedId<TalentId>;

public class Talent : SoftDeletableAggregate<TalentId>
{
    public string Description { get; private set; }
    public ProfileId ProfileId { get; private set; }

    private Talent() : base() { }

    private Talent(TalentId id, ProfileId profileId, IClock clock, string description) : base(id, clock)
    {
        Guard.IsNotNullOrWhiteSpace(description);

        Id = id;
        Description = description;
        ProfileId = profileId;
    }

    public static Talent CreateInstance(IGuidGenerator guidGenerator, IClock clock, ProfileId profileId, string description)
    {
        var id = new TalentId(guidGenerator.NewGuid());
        return new Talent( id, profileId,clock, description);
    }
}