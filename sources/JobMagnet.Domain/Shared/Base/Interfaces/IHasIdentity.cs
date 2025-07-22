namespace JobMagnet.Domain.Shared.Base.Interfaces;

public interface IHasIdentity<out TId>
{
    TId Id { get; }
}