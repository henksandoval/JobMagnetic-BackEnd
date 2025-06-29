using CommunityToolkit.Diagnostics;
using JobMagnet.Domain.Aggregates.Profiles.Entities;
using JobMagnet.Domain.Core.Enums;
using JobMagnet.Domain.Services;
using JobMagnet.Shared.Abstractions;

namespace JobMagnet.Domain.Aggregates.Profiles.ValueObjects;

public class VanityUrlManager
{
    private readonly Profile _profile;

    private VanityUrlManager()
    {
    }

    internal VanityUrlManager(Profile profile)
    {
        _profile = profile;
    }

    public void CreateAndAssignPublicIdentifier(IGuidGenerator guidGenerator, IClock clock, IProfileSlugGenerator slugGenerator)
    {
        if (_profile.VanityUrls.Any(p => p.Type == LinkType.Primary)) return;

        var generatedSlug = slugGenerator.GenerateProfileSlug(_profile);
        AddPublicProfileIdentifier(guidGenerator, clock, generatedSlug);
    }

    public void AddPublicProfileIdentifier(IGuidGenerator guidGenerator, IClock clock, string slug, LinkType type = LinkType.Primary)
    {
        Guard.IsNotNullOrEmpty(slug);

        var publicIdentifier = VanityUrl.CreateInstance(guidGenerator, clock, _profile.Id, slug, type);

        _profile.AddPublicProfileIdentifier(publicIdentifier);
    }
}