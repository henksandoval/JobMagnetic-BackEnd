namespace JobMagnet.Domain.Entities.Base.Interfaces;

public interface IHasIdentity<TId>
{
    TId Id { get; set; }
}