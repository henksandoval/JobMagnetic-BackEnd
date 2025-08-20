using JobMagnet.Domain.Aggregates.Profiles.ValueObjects;
using JobMagnet.Domain.Shared.Base.Entities;

namespace JobMagnet.Domain.Aggregates;

public class User  : SoftDeletableEntity<UserId>
{
    public string Email { get; set; }
    public string PhotoUrl { get; set; }
}