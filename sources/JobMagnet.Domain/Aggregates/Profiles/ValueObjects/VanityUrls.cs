using CommunityToolkit.Diagnostics;
using JobMagnet.Domain.Aggregates.Profiles.Entities;
using JobMagnet.Domain.Core.Enums;
using JobMagnet.Domain.Services;

namespace JobMagnet.Domain.Aggregates.Profiles.ValueObjects;

public class VanityUrls
{
    private readonly Profile _profile;

    private VanityUrls()
    {
    }

    internal VanityUrls(Profile profile)
    {
        _profile = profile;
    }

    public void CreateAndAssignPublicIdentifier(IProfileSlugGenerator slugGenerator)
    {
        if (_profile.VanityUrls.Any(p => p.Type == LinkType.Primary)) return;

        var generatedSlug = slugGenerator.GenerateProfileSlug(_profile);
        AddPublicProfileIdentifier(generatedSlug);
    }

    public void AddPublicProfileIdentifier(string slug, LinkType type = LinkType.Primary)
    {
        Guard.IsNotNullOrEmpty(slug);

        var publicIdentifier = new VanityUrl(slug, _profile.Id, type);

        _profile.AddPublicProfileIdentifier(publicIdentifier);
    }
}