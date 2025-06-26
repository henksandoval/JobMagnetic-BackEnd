using CommunityToolkit.Diagnostics;
using JobMagnet.Domain.Aggregates.Profiles.Entities;
using JobMagnet.Domain.Exceptions;

namespace JobMagnet.Domain.Aggregates.Profiles.ValueObjects;

public class TalentShowcase
{
    private readonly Profile _profile;
    private IReadOnlyCollection<Talent> Talents => _profile.Talents;

    private TalentShowcase()
    {
    }

    internal TalentShowcase(Profile profile)
    {
        Guard.IsNotNull(profile);
        _profile = profile;
    }

    public void AddTalent(string description)
    {
        if (Talents.Count >= 10) throw new JobMagnetDomainException("Cannot add more than 10 talents.");
        if (Talents.Any(t => t.Description == description)) throw new JobMagnetDomainException("This talent already exists.");

        var talent = new Talent(description, _profile.Id, new TalentId());

        _profile.AddTalent(talent);
    }
}