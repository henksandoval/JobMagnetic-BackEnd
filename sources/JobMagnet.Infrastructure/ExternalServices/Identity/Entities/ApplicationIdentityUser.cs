using JobMagnet.Domain.Aggregates;
using Microsoft.AspNetCore.Identity;

namespace JobMagnet.Infrastructure.ExternalServices.Identity.Entities;

public class ApplicationIdentityUser : IdentityUser<Guid>
{
    public virtual User User { get; set; }
}