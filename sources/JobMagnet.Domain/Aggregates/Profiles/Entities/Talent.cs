using CommunityToolkit.Diagnostics;
using JobMagnet.Domain.Aggregates.Profiles.ValueObjects;
using JobMagnet.Domain.Exceptions;
using JobMagnet.Domain.Shared.Base.Entities;
using JobMagnet.Shared.Abstractions;

namespace JobMagnet.Domain.Aggregates.Profiles.Entities;

public class Talent : TrackableEntity<TalentId>
{
    public const int MaxNameLength = 50;
    public ProfileId ProfileId { get; private set; }
    public string Description { get; private set; }
    
    private Talent() { }
    
    private Talent(TalentId id, ProfileId profileId, string description)
    {
        Guard.IsNotNullOrWhiteSpace(description);
        Guard.IsLessThanOrEqualTo(description.Length, MaxNameLength);
        
        Id = id;
        Description = description;
        ProfileId = profileId;
    }

    public static Talent CreateInstance(IGuidGenerator guidGenerator, ProfileId profileId, string description)
    {
        var id = new TalentId(guidGenerator.NewGuid());
        return new Talent(id, profileId, description);
    }
    
    internal void UpdateDetails(string description)
    {
        Description = description;
        
        ValidateInvariants();
    }
    
    private void ValidateInvariants()
    {
        Guard.IsNotNullOrEmpty(Description);

        if (string.IsNullOrEmpty(Description))
            throw new JobMagnetDomainException("The description cannot be empty or null.");
    }
}