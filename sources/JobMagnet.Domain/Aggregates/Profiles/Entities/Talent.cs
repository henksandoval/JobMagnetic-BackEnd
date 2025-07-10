using CommunityToolkit.Diagnostics;
using JobMagnet.Domain.Aggregates.Profiles.ValueObjects;
using JobMagnet.Domain.Exceptions;

namespace JobMagnet.Domain.Aggregates.Profiles.Entities;

public record Talent
{
    public const int MaxNameLength = 50;
    public Guid Id { get; private set; }
    public ProfileId ProfileId { get; private set; }
    public string Description { get; private set; }
    
    private Talent() { }

    // TalentId id,
   // Id = id.Value;
    private Talent(string description)
    {
        Guard.IsNotNullOrWhiteSpace(description);
        Guard.IsLessThanOrEqualTo(description.Length, MaxNameLength);
        
        Description = description;
    }

    public static Talent CreateInstance(string description)
    {
        return new Talent(description);
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