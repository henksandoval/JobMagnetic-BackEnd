using System.Diagnostics.CodeAnalysis;
using CommunityToolkit.Diagnostics;
using JobMagnet.Domain.Exceptions;
using JobMagnet.Domain.Shared.Base;

namespace JobMagnet.Domain.Aggregates.Profiles.Entities;

public readonly record struct ProjectId(Guid Value) : IStronglyTypedId<Guid>;

public class Project : SoftDeletableEntity<ProjectId>
{
    public int Position { get; }
    public string Title { get; }
    public string Description { get; }
    public string UrlLink { get; }
    public string UrlImage { get; }
    public string UrlVideo { get; }
    public string Type { get; }
    public ProfileId ProfileId { get; private set; }

    private Project() : base(new ProjectId(), Guid.Empty)
    {
    }

    [SetsRequiredMembers]
    public Project(string title, string description, string urlLink, string urlImage, string urlVideo, string type, int position, ProfileId profileId,
        ProjectId id) : base(id, Guid.Empty)
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