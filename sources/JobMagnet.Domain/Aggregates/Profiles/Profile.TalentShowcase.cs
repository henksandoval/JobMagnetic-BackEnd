using JobMagnet.Domain.Aggregates.Profiles.Entities;
using JobMagnet.Domain.Aggregates.Profiles.ValueObjects;
using JobMagnet.Domain.Exceptions;
using JobMagnet.Shared.Abstractions;

namespace JobMagnet.Domain.Aggregates.Profiles;

public partial class Profile
{
    public Talent AddTalent(IGuidGenerator guidGenerator, string description) =>
        AddTalentToTalentShowcase(guidGenerator, description);
    
    private Talent AddTalentToTalentShowcase(IGuidGenerator guidGenerator, string description)
    {
        if (TalentShowcase.Count >= 10) throw new JobMagnetDomainException("Cannot add more than 10 talents.");
        if (TalentShowcase.Any(t => t.Description == description))
            throw new JobMagnetDomainException("This talent already exists.");

        var talent = Talent.CreateInstance(guidGenerator, Id, description);
        _talentShowcase.Add(talent);

        return talent;
    }
    
    public void UpdateTalent(TalentId talentId, string description)
    {
        UpdateTalentInTalentShowcase(
            talentId,
            description
        );
    }
    
    private void UpdateTalentInTalentShowcase(TalentId talentId, string description)
    {
        var updatedTalent = TalentShowcase.FirstOrDefault(t => t.Id == talentId);
        if (updatedTalent is null)
            throw NotFoundException.For<Talent, TalentId>(talentId);
        
        if (TalentShowcase.Any(t => t.Description == description && t.Id != talentId))
            throw new JobMagnetDomainException("A Talent with this title already exists in the talent show case.");
        
        updatedTalent.UpdateDetails(description);
        
    }
}

