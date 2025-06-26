using System.Diagnostics.CodeAnalysis;
using CommunityToolkit.Diagnostics;
using JobMagnet.Domain.Core.Enums;
using JobMagnet.Domain.Shared.Base;

namespace JobMagnet.Domain.Aggregates.Profiles.Entities;

public readonly record struct VanityUrlId(Guid Value) : IStronglyTypedId<Guid>;

public class VanityUrl : TrackableEntity<VanityUrlId>
{
    public const int MaxNameLength = 25;
    public const string DefaultSlug = "profile";

    public string ProfileSlugUrl { get; private set; } = null!;
    public LinkType Type { get; private set; }
    public ProfileId ProfileId { get; private set; }
    public long ViewCount { get; private set; }

    private VanityUrl() : base(new VanityUrlId(), Guid.Empty)
    {
    }

    [SetsRequiredMembers]
    internal VanityUrl(string slug, ProfileId profileId, LinkType linkType = LinkType.Primary, VanityUrlId id = default)
        : base(id, Guid.Empty)
    {
        Guard.IsNotNullOrEmpty(slug);

        Id = id;
        ProfileId = profileId;
        ProfileSlugUrl = slug;
        Type = linkType;
        ViewCount = 0;
    }
}