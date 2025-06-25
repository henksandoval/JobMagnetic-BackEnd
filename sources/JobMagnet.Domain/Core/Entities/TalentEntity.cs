using System.Diagnostics.CodeAnalysis;
using CommunityToolkit.Diagnostics;
using JobMagnet.Domain.Core.Entities.Base;

namespace JobMagnet.Domain.Core.Entities;

public class TalentEntity : SoftDeletableEntity<long>
{
    public string Description { get; private set; }
    public long ProfileId { get; private set; }

    private TalentEntity()
    {
    }

    [SetsRequiredMembers]
    public TalentEntity(string description, long profileId = 0, long id = 0)
    {
        Guard.IsGreaterThanOrEqualTo(id, 0);
        Guard.IsGreaterThanOrEqualTo(profileId, 0);
        Guard.IsNotNullOrWhiteSpace(description);

        Id = id;
        Description = description;
        ProfileId = profileId;
    }
}