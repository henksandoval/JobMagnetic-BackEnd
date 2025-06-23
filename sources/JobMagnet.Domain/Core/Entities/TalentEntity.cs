using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using CommunityToolkit.Diagnostics;
using JobMagnet.Domain.Core.Entities.Base;

namespace JobMagnet.Domain.Core.Entities;

public class TalentEntity : SoftDeletableEntity<long>
{
    public string Description { get; private set; } 
    public long ProfileId { get; private set; }
    public virtual ProfileEntity Profile { get; private set; }
    private TalentEntity() { }

    [SetsRequiredMembers]
    internal TalentEntity(string description, long profileId, ProfileEntity profileEntity, long id = 0)
    {
        Guard.IsGreaterThanOrEqualTo(id, 0);
        Guard.IsGreaterThanOrEqualTo(profileId, 0);
        Guard.IsNotNullOrWhiteSpace(description);
        Guard.IsNotNull(profileEntity);
        
        Id = id;
        Description = description;
        ProfileId = profileId;
        Profile = profileEntity;
    }
}