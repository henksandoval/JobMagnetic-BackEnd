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
    public string PhotoUrl { get; private set; }
    public virtual IReadOnlyCollection<RefreshToken> RefreshTokens => _refreshTokens;

    private User() { }
    
    private  static User CreateInstance(UserId id, string email, string photoUrl)
    {
        Guard.IsNotNullOrWhiteSpace(email);
        Guard.IsNotNullOrWhiteSpace(photoUrl);

        return new User
        {
            Id = id,
            Email = email,
            PhotoUrl = photoUrl
        };
    }

    public  RefreshToken AddRefreshToken(IGuidGenerator guidGenerator, string token, TimeSpan validity, int maxActiveTokens)
    {
        if (_refreshTokens.Count(rt => rt.IsActive) >= maxActiveTokens)
            throw new JobMagnetDomainException($"Cannot have more than {maxActiveTokens} active sessions.");
        
        var newRefreshToken = RefreshToken.CreateInstance(guidGenerator, Id, token, validity);
        _refreshTokens.Add(newRefreshToken);
        
        return newRefreshToken;
    }
    
}