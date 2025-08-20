using JobMagnet.Domain.Aggregates;
using Microsoft.AspNetCore.Identity;

namespace JobMagnet.Infrastructure.Services.Auth.Entities;

public class ApplicationIdentityUser : IdentityUser<Guid>
{
    public User UserExtended { get; set; }
}