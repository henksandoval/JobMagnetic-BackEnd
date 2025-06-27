using System.Diagnostics.CodeAnalysis;
using CommunityToolkit.Diagnostics;
using JobMagnet.Domain.Exceptions;
using JobMagnet.Domain.Shared.Base;
using JobMagnet.Domain.Shared.Base.Interfaces;
using JobMagnet.Shared.Abstractions;

namespace JobMagnet.Domain.Aggregates.Profiles.Entities;

public readonly record struct ProjectId(Guid Value) : IStronglyTypedId<ProjectId>;

public class Project : SoftDeletableAggregate<ProjectId>
{
    public int Position { get; }
    public string Title { get; }
    public string Description { get; }
    public string UrlLink { get; }
    public string UrlImage { get; }
    public string UrlVideo { get; }
    public string Type { get; }
    public ProfileId ProfileId { get; private set; }

    private Project(ProjectId id, DateTimeOffset addedAt, DateTimeOffset? lastModifiedAt, DateTimeOffset? deletedAt) :
        base(id, addedAt, lastModifiedAt, deletedAt)
    {
    }

    private Project(ProjectId id, ProfileId profileId, IClock clock, string title, string description, string urlLink, string urlImage,
        string urlVideo,
        string type, int position) : base(id, clock)
    {
        Id = id;
        Title = title;
        Description = description;
        UrlLink = urlLink;
        UrlImage = urlImage;
        UrlVideo = urlVideo;
        Type = type;
        ProfileId = profileId;
        Position = position;

        ValidateInvariants();
    }

    public static Project CreateInstance(IGuidGenerator guidGenerator, IClock clock, ProfileId profileId, string title, string description, string urlLink, string urlImage, string urlVideo, string type, int position)
    {
        var id = new ProjectId(guidGenerator.NewGuid());
        return new Project(id, profileId, clock, title, description, urlLink, urlImage, urlVideo, type, position);
    }

    private void ValidateInvariants()
    {
        Guard.IsGreaterThanOrEqualTo(Position, 0);
        Guard.IsNotNullOrEmpty(Title);
        Guard.IsNotNullOrEmpty(Description);
        Guard.IsNotNullOrEmpty(Type);

        if (string.IsNullOrEmpty(UrlLink) && string.IsNullOrEmpty(UrlImage) && string.IsNullOrEmpty(UrlVideo))
            throw new JobMagnetDomainException("At least one of UrlLink, UrlImage, or UrlVideo must be provided.");
    }
}