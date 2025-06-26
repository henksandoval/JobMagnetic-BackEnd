using CommunityToolkit.Diagnostics;
using JobMagnet.Domain.Core.Enums;
using JobMagnet.Domain.Services;

namespace JobMagnet.Domain.Core.Entities;

public class VanityUrls
{
    private readonly ProfileEntity _profile;

    private VanityUrls()
    {
    }

    internal VanityUrls(ProfileEntity profile)
    {
        _profile = profile;
    }

    public void CreateAndAssignPublicIdentifier(IProfileSlugGenerator slugGenerator)
    {
        if (_profile.PublicProfileIdentifiers.Any(p => p.Type == LinkType.Primary)) return;

        var generatedSlug = slugGenerator.GenerateProfileSlug(_profile);
        AddPublicProfileIdentifier(generatedSlug);
    }

    public void AddPublicProfileIdentifier(string slug, LinkType type = LinkType.Primary)
    {
        Guard.IsNotNullOrEmpty(slug);

        var publicIdentifier = new PublicProfileIdentifierEntity(slug, _profile.Id, type);

        _profile.AddPublicProfileIdentifier(publicIdentifier);
    }
}