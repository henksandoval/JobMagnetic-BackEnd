using System.Diagnostics.CodeAnalysis;
using CommunityToolkit.Diagnostics;
using JobMagnet.Domain.Core.Entities.Base;
using JobMagnet.Domain.Exceptions;

namespace JobMagnet.Domain.Core.Entities;

public class Project : SoftDeletableEntity<long>
{
    public int Position { get; }
    public string Title { get; }
    public string Description { get; }
    public string UrlLink { get; }
    public string UrlImage { get; }
    public string UrlVideo { get; }
    public string Type { get; }
    public long ProfileId { get; }

    private Project()
    {
    }

    [SetsRequiredMembers]
    public Project(string title, string description, string urlLink, string urlImage, string urlVideo, string type, int position, long profileId = 0,
        long id = 0)
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
        Guard.IsGreaterThanOrEqualTo(Id, 0);
        Guard.IsGreaterThanOrEqualTo(ProfileId, 0);
        Guard.IsGreaterThanOrEqualTo(Position, 0);
        Guard.IsNotNullOrEmpty(Title);
        Guard.IsNotNullOrEmpty(Description);
        Guard.IsNotNullOrEmpty(Type);

        if (string.IsNullOrEmpty(UrlLink) && string.IsNullOrEmpty(UrlImage) && string.IsNullOrEmpty(UrlVideo))
            throw new JobMagnetDomainException("At least one of UrlLink, UrlImage, or UrlVideo must be provided.");
    }
}