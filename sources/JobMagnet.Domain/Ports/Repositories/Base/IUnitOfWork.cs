namespace JobMagnet.Domain.Ports.Repositories.Base;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}