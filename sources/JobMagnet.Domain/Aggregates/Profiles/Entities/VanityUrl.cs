using CommunityToolkit.Diagnostics;
using JobMagnet.Domain.Aggregates.Profiles.ValueObjects;
using JobMagnet.Domain.Enums;
using JobMagnet.Domain.Shared.Base.Entities;
using JobMagnet.Shared.Abstractions;

namespace JobMagnet.Domain.Aggregates.Profiles.Entities;

public class VanityUrl : TrackableEntity<VanityUrlId>
{
    public const int MaxNameLength = 25;
    public const string DefaultSlug = "profile";

    public string ProfileSlugUrl { get; private set; } = null!;
    public LinkType Type { get; private set; }
    public ProfileId ProfileId { get; private set; }
    public long ViewCount { get; private set; }

    private VanityUrl()
    {
    }

    private VanityUrl(VanityUrlId id, ProfileId profileId, string slug, LinkType linkType = LinkType.Primary)
        : base(id)
    {
        Guard.IsNotNullOrEmpty(slug);

        Id = id;
        ProfileId = profileId;
        ProfileSlugUrl = slug;
        Type = linkType;
        ViewCount = 0;
    }

    internal static VanityUrl CreateInstance(IGuidGenerator guidGenerator, ProfileId profileId, string slug,
        LinkType linkType = LinkType.Primary)
    {
        var id = new VanityUrlId(guidGenerator.NewGuid());
        return new VanityUrl(id, profileId, slug, linkType);
    }
}