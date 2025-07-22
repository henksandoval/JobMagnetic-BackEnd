namespace JobMagnet.Domain.Shared.Base.Interfaces;

public interface IAuditableEntity
{
    DateTimeOffset AddedAt { get; }
    DateTimeOffset? LastModifiedAt { get; }
}