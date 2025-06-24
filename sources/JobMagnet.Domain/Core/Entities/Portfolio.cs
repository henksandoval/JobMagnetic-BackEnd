using CommunityToolkit.Diagnostics;
using JobMagnet.Domain.Exceptions;

namespace JobMagnet.Domain.Core.Entities;

public class Portfolio
{
    private readonly ProfileEntity _profile ;
    private IReadOnlyCollection<PortfolioGalleryEntity> Gallery => _profile.PortfolioGallery;

    public Portfolio(ProfileEntity profile)
    {
        Guard.IsNotNull(profile);
        _profile = profile;
    }

    public void AddGallery(string title, string description, string urlLink, string urlImage, string urlVideo, string type)
    {
        if (Gallery.Count >= 20)
        {
            throw new JobMagnetDomainException("Cannot add more than 20 testimonials.");
        }

        var gallery = new PortfolioGalleryEntity( title, description, urlLink, urlImage, urlVideo, type, _profile.Id);

        _profile.AddPortfolio(gallery);
    }
}