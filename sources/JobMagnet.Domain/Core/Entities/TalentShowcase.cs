using CommunityToolkit.Diagnostics;
using JobMagnet.Domain.Exceptions;

namespace JobMagnet.Domain.Core.Entities;

public class TalentShowcase
{
    private readonly ProfileEntity _profile;
    private IReadOnlyCollection<TalentEntity> Talents => _profile.Talents;
    
    internal TalentShowcase(ProfileEntity profile)
    {
        Guard.IsNotNull(profile);
        _profile = profile;
    }
    public void AddTalent(string description)
    {
        if (Talents.Count >= 20)
        {
            throw new JobMagnetDomainException("Cannot add more than 20 talents.");
        }
        if (Talents.Any(t => t.Description == description))
        {
            throw new JobMagnetDomainException("This talent already exists.");
        }
        
        var talent = new TalentEntity(description, _profile.Id);
        
        _profile.AddTalent(talent);
    }
}