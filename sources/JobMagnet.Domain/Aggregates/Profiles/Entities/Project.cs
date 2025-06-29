using CommunityToolkit.Diagnostics;
using JobMagnet.Domain.Exceptions;
using JobMagnet.Domain.Shared.Base;
using JobMagnet.Domain.Shared.Base.Interfaces;
using JobMagnet.Shared.Abstractions;

namespace JobMagnet.Domain.Aggregates.Profiles.Entities;

public readonly record struct ProjectId(Guid Value) : IStronglyTypedId<ProjectId>;

public class Project : SoftDeletableAggregate<ProjectId>
{
    public int Position { get; private set; }
    public string Title { get; private set; }
    public string Description { get; private set; }
    public string UrlLink { get; private set; }
    public string UrlImage { get; private set; }
    public string UrlVideo { get; private set; }
    public string Type { get; private set; }
    public ProfileId ProfileId { get; private set; }

    private Project() : base() { }

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

    internal static Project CreateInstance(IGuidGenerator guidGenerator, IClock clock, ProfileId profileId, string title, string description, string urlLink, string urlImage, string urlVideo, string type, int position)
    {
        var id = new ProjectId(guidGenerator.NewGuid());
        return new Project(id, profileId, clock, title, description, urlLink, urlImage, urlVideo, type, position);
    }

    internal Project UpdateDetails(string newTitle, string newDescription, string newUrlLink, string newUrlImage, string newUrlVideo, string newType)
    {
        Title = newTitle;
        Description = newDescription;
        UrlLink = newUrlLink;
        UrlImage = newUrlImage;
        UrlVideo = newUrlVideo;
        Type = newType;

        ValidateInvariants();

        return this;
    }

    internal void UpdatePosition(int newPosition)
    {
        Guard.IsGreaterThanOrEqualTo(newPosition, 1);
        Position = newPosition;
    }

    private void ValidateInvariants()
    {
        Guard.IsGreaterThanOrEqualTo(Position, 1);
        Guard.IsNotNullOrEmpty(Title);
        Guard.IsNotNullOrEmpty(Description);
        Guard.IsNotNullOrEmpty(Type);

        if (string.IsNullOrEmpty(UrlLink) && string.IsNullOrEmpty(UrlImage) && string.IsNullOrEmpty(UrlVideo))
            throw new JobMagnetDomainException("At least one of UrlLink, UrlImage, or UrlVideo must be provided.");
    }
}