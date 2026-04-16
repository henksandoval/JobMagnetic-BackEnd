using System.ComponentModel.DataAnnotations.Schema;
using CommunityToolkit.Diagnostics;
using JobMagnet.Domain.Aggregates.Auth.ValueObjects;
using JobMagnet.Domain.Exceptions;
using JobMagnet.Domain.Shared.Base.Entities;
using JobMagnet.Shared.Abstractions;

namespace JobMagnet.Domain.Aggregates.Auth.Entities;

public class User : SoftDeletableEntity<UserId>
{
    private readonly HashSet<RefreshToken> _refreshTokens = [];
    public string Email { get; private  set; }
    [NotMapped]
    public string DisplayName { get; set; }
    public string? PhotoUrl { get; private set; }
    public Guid ApplicationIdentityUserId { get;  set; }
    public virtual IReadOnlyCollection<RefreshToken> RefreshTokens => _refreshTokens;

    private User() { }
    
    public static  User AddUser(UserId id, string email, string displayName,  string? photoUrl, Guid applicationIdentityUserId)
    {
        Guard.IsNotNullOrWhiteSpace(email);
        Guard.IsNotDefault(applicationIdentityUserId);
        return new User
        {
            Id = id,
            Email = email,
            DisplayName = displayName,
            PhotoUrl = photoUrl,
            ApplicationIdentityUserId = applicationIdentityUserId,
        };
    }
    
    public  RefreshToken AddRefreshToken(IGuidGenerator guidGenerator, string token, TimeSpan validity, int maxActiveTokens = 1)
    {
        if (_refreshTokens.Count(rt => rt.IsActive) >= maxActiveTokens)
            throw new JobMagnetDomainException($"Cannot have more than {maxActiveTokens} active sessions.");
        
        var newRefreshToken = RefreshToken.CreateInstance(guidGenerator, Id, token, validity);
        _refreshTokens.Add(newRefreshToken);
        
        return newRefreshToken;
    }
    
    public void RevokeRefreshToken(string token)
    {
        var tokenToRevoke = _refreshTokens.FirstOrDefault(rt => rt.Token == token);
        if (tokenToRevoke is null || !tokenToRevoke.IsActive)
            return;
        tokenToRevoke.Revoke();
    }
}