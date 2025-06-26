using System.Diagnostics.CodeAnalysis;
using CommunityToolkit.Diagnostics;
using JobMagnet.Domain.Core.Entities.Base;
using JobMagnet.Domain.Core.Enums;

namespace JobMagnet.Domain.Aggregates.Profiles.Entities;

public class VanityUrl : TrackableEntity<long>
{
    public const int MaxNameLength = 25;
    public const string DefaultSlug = "profile";

    public string ProfileSlugUrl { get; private set; } = null!;
    public LinkType Type { get; private set; }
    public long ProfileId { get; private set; }
    public long ViewCount { get; private set; }

    private VanityUrl()
    {
    }

    [SetsRequiredMembers]
    internal VanityUrl(string slug, long profileEntityId, LinkType linkType = LinkType.Primary, long id = 0)
    {
        Guard.IsNotNullOrEmpty(slug);
        Guard.IsGreaterThanOrEqualTo(profileEntityId, 0);

        Id = id;
        ProfileId = profileEntityId;
        ProfileSlugUrl = slug;
        Type = linkType;
        ViewCount = 0;
    }
}