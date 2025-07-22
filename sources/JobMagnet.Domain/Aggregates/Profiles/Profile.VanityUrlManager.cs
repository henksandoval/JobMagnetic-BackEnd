using CommunityToolkit.Diagnostics;
using JobMagnet.Domain.Aggregates.Profiles.Entities;
using JobMagnet.Domain.Enums;
using JobMagnet.Domain.Exceptions;
using JobMagnet.Domain.Services;
using JobMagnet.Shared.Abstractions;

namespace JobMagnet.Domain.Aggregates.Profiles;

public partial class Profile
{
    public void AssignDefaultVanityUrl(IGuidGenerator guidGenerator, IProfileSlugGenerator slugGenerator)
    {
        ExecuteAssignDefaultVanityUrl(guidGenerator, slugGenerator);
    }

    public void AddVanityUrl(IGuidGenerator guidGenerator, string slug, LinkType type = LinkType.Primary)
    {
        ExecuteAddVanityUrl(guidGenerator, slug, type);
    }

    private void ExecuteAssignDefaultVanityUrl(IGuidGenerator guidGenerator, IProfileSlugGenerator slugGenerator)
    {
        if (_vanityUrls.Any(p => p.Type == LinkType.Primary)) return;

        var generatedSlug = slugGenerator.GenerateProfileSlug(this);
        ExecuteAddVanityUrl(guidGenerator, generatedSlug);
    }

    private void ExecuteAddVanityUrl(IGuidGenerator guidGenerator, string slug, LinkType type = LinkType.Primary)
    {
        Guard.IsNotNullOrEmpty(slug);

        if (_vanityUrls.Any(url => url.ProfileSlugUrl.Equals(slug, StringComparison.OrdinalIgnoreCase)))
            throw new JobMagnetDomainException($"A vanity URL with the slug '{slug}' already exists for this profile.");
        if (type == LinkType.Primary && _vanityUrls.Any(url => url.Type == LinkType.Primary))
            throw new JobMagnetDomainException("This profile already has a primary vanity URL. Cannot add another one.");

        var publicIdentifier = VanityUrl.CreateInstance(guidGenerator, Id, slug, type);

        _vanityUrls.Add(publicIdentifier);
    }
}