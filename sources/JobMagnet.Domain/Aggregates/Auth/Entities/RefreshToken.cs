using JobMagnet.Domain.Aggregates.Auth.ValueObjects;
using JobMagnet.Domain.Aggregates.Profiles.ValueObjects;
using JobMagnet.Domain.Shared.Base.Entities;
using JobMagnet.Shared.Abstractions;

namespace JobMagnet.Domain.Aggregates.Auth.Entities;

public class RefreshToken : TrackableEntity<RefreshTokenId>
{
    public UserId UserId { get; private  set; }
    public string Token { get; private  set; }
    public DateTime Expires { get; private  set; }
    public DateTime Created { get; private  set; }
    public DateTime? Revoked { get; private  set; }
    public  bool IsExpired => DateTime.UtcNow >= Expires;
    public bool IsActive => Revoked == null && !IsExpired;
    
    private RefreshToken() { }
    
    internal   static RefreshToken CreateInstance(IGuidGenerator guidGenerator, UserId userId, string token, TimeSpan validity)
    {
        return new RefreshToken
        {
            Id = new RefreshTokenId(guidGenerator.NewGuid()),
            UserId = userId,
            Token = token,
            Created = DateTime.UtcNow,
            Expires = DateTime.UtcNow.Add(validity)
        };
    }
     
    internal void Revoke()
    {
        if (IsActive)
        {
            Revoked = DateTime.UtcNow;
        }
    }
}