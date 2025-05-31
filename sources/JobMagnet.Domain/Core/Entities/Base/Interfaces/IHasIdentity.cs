namespace JobMagnet.Domain.Core.Entities.Base.Interfaces;

public interface IHasIdentity<TId>
{
    TId Id { get; set; }
}