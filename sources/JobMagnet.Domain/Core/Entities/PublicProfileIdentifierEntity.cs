using System.Diagnostics.CodeAnalysis;
using CommunityToolkit.Diagnostics;
using JobMagnet.Domain.Core.Entities.Base;
using JobMagnet.Domain.Core.Enums;

namespace JobMagnet.Domain.Core.Entities;

public class PublicProfileIdentifierEntity : TrackableEntity<long>
{
    public const int MaxNameLength = 25;
    public const string DefaultSlug = "profile";

    public string ProfileSlugUrl { get; private set; } = null!;
    public LinkType Type { get; private set; }
    public long ProfileId { get; private set; }
    public long ViewCount { get; private set; }

    private PublicProfileIdentifierEntity() { }

    [SetsRequiredMembers]
    internal PublicProfileIdentifierEntity(string slug, long profileEntityId)
    {
        Guard.IsNotNullOrEmpty(slug);
        Guard.IsGreaterThanOrEqualTo(profileEntityId, 0);

        ProfileId = profileEntityId;
        ProfileSlugUrl = slug;
        Type = LinkType.Primary;
        ViewCount = 0;
    }
}