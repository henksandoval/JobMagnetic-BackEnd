using CommunityToolkit.Diagnostics;
using JobMagnet.Domain.Core.Enums;
using JobMagnet.Domain.Shared.Base;
using JobMagnet.Domain.Shared.Base.Aggregates;
using JobMagnet.Domain.Shared.Base.Interfaces;
using JobMagnet.Shared.Abstractions;

namespace JobMagnet.Domain.Aggregates.Profiles.Entities;

public readonly record struct VanityUrlId(Guid Value) : IStronglyTypedId<VanityUrlId>;

public class VanityUrl : TrackableAggregate<VanityUrlId>
{
    public const int MaxNameLength = 25;
    public const string DefaultSlug = "profile";

    public string ProfileSlugUrl { get; private set; } = null!;
    public LinkType Type { get; private set; }
    public ProfileId ProfileId { get; private set; }
    public long ViewCount { get; private set; }

    private VanityUrl() : base() {}

    private VanityUrl(VanityUrlId id, ProfileId profileId, IClock clock, string slug, LinkType linkType = LinkType.Primary)
        : base(id, clock)
    {
        Guard.IsNotNullOrEmpty(slug);

        Id = id;
        ProfileId = profileId;
        ProfileSlugUrl = slug;
        Type = linkType;
        ViewCount = 0;
    }

    internal static VanityUrl CreateInstance(IGuidGenerator guidGenerator, IClock clock, ProfileId profileId, string slug,
        LinkType linkType = LinkType.Primary)
    {
        var id = new VanityUrlId(guidGenerator.NewGuid());
        return new VanityUrl(id, profileId, clock, slug, linkType);
    }
}