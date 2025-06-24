using System.Diagnostics.CodeAnalysis;
using CommunityToolkit.Diagnostics;
using JobMagnet.Domain.Core.Entities.Base;
using JobMagnet.Domain.Exceptions;

namespace JobMagnet.Domain.Core.Entities;

// ReSharper disable once ClassNeverInstantiated.Global
public class PortfolioGalleryEntity : SoftDeletableEntity<long>
{
    public int Position { get; private set; }
    public string Title { get; private set; }
    public string Description { get; private set; }
    public string UrlLink { get; private set; }
    public string UrlImage { get; private set; }
    public string UrlVideo { get; private set; }
    public string Type { get; private set; }
    public long ProfileId { get; private set; }

    [SetsRequiredMembers]
    public PortfolioGalleryEntity(string title, string description, string urlLink, string urlImage, string urlVideo, string type, long profileId = 0, long id = 0)
    {
        Id = id;
        Title = title;
        Description = description;
        UrlLink = urlLink;
        UrlImage = urlImage;
        UrlVideo = urlVideo;
        Type = type;
        ProfileId = profileId;

        ValidateInvariants();
    }

    private void ValidateInvariants()
    {
        Guard.IsGreaterThanOrEqualTo(Id, 0);
        Guard.IsGreaterThanOrEqualTo(ProfileId, 0);
        Guard.IsNotNullOrEmpty(Title);
        Guard.IsNotNullOrEmpty(Description);
        Guard.IsNotNullOrEmpty(Type);

        if (string.IsNullOrEmpty(UrlLink) && string.IsNullOrEmpty(UrlImage) && string.IsNullOrEmpty(UrlVideo))
        {
            throw new JobMagnetDomainException("At least one of UrlLink, UrlImage, or UrlVideo must be provided.");
        }
    }
}