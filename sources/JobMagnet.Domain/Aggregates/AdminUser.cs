using JobMagnet.Domain.Shared.Base.Entities;

namespace JobMagnet.Domain.Aggregates;

public class AdminUser
{
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}