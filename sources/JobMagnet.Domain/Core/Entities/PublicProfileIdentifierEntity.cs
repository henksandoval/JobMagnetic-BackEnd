using System.Diagnostics.CodeAnalysis;
using JobMagnet.Domain.Core.Entities.Base;
using JobMagnet.Domain.Core.Enums;
using JobMagnet.Domain.Services;

namespace JobMagnet.Domain.Core.Entities;

public class PublicProfileIdentifierEntity : TrackableEntity<long>
{
    public const int MaxNameLength = 25;
    public const string DefaultSlug = "profile";

    public string ProfileSlugUrl { get; private set; } = null!;
    public LinkType Type { get; private set; }
    public long ProfileId { get; private set; }
    public long ViewCount { get; private set; }

    public ProfileEntity ProfileEntity { get; init; } = null!;

    private PublicProfileIdentifierEntity() { }

    [SetsRequiredMembers]
    public PublicProfileIdentifierEntity(ProfileEntity profileEntity, string slug)
    {
        ArgumentNullException.ThrowIfNull(profileEntity, nameof(profileEntity));
        ArgumentNullException.ThrowIfNull(slug, nameof(slug));

        ProfileId = profileEntity.Id;
        ProfileEntity = profileEntity;
        ProfileSlugUrl = slug;
        Type = LinkType.Primary;
        ViewCount = 0;
    }
}