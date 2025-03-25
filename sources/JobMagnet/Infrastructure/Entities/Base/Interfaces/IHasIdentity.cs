namespace JobMagnet.Infrastructure.Entities.Base.Interfaces;

public interface IHasIdentity<TId>
{
    TId Id { get; set; }
}