using System.Diagnostics.CodeAnalysis;
using JobMagnet.Domain.Core.Entities.Base;
using JobMagnet.Domain.Core.Enums;
using JobMagnet.Domain.Services;

namespace JobMagnet.Domain.Core.Entities;

public class PublicProfileIdentifierEntity : TrackableEntity<long>
{
    public const int MaxNameLength = 25;
    public const string DefaultIdentifierName = "profile";

    public string Identifier { get; private set; } = null!;
    public LinkType Type { get; private set; }
    public long ProfileId { get; private set; }
    public long ViewCount { get; private set; }

    public virtual ProfileEntity ProfileEntity { get; private set; } = null!;

    public PublicProfileIdentifierEntity() { }

    [SetsRequiredMembers]
    public PublicProfileIdentifierEntity(ProfileEntity profileEntity, IProfileIdentifierNameGenerator identifierNameGenerator)
    {
        ArgumentNullException.ThrowIfNull(profileEntity, nameof(profileEntity));
        ArgumentNullException.ThrowIfNull(identifierNameGenerator, nameof(identifierNameGenerator));

        ProfileId = profileEntity.Id;
        ProfileEntity = profileEntity;
        Identifier = identifierNameGenerator.GenerateIdentifierName(profileEntity);
        Type = LinkType.Primary;
        ViewCount = 0;
    }
}