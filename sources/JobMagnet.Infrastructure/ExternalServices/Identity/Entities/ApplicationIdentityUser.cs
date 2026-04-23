using JobMagnet.Domain.Aggregates;
using JobMagnet.Domain.Aggregates.Auth.Entities;
using Microsoft.AspNetCore.Identity;

namespace JobMagnet.Infrastructure.ExternalServices.Identity.Entities;

public class ApplicationIdentityUser : IdentityUser<Guid>
{
    public virtual User User { get; set; }
}