using Microsoft.EntityFrameworkCore.Storage;

namespace JobMagnet.Domain.Domain.Ports.Repositories.Base;

public interface ICommandRepository<in TEntity> where TEntity : class
{
    Task CreateAsync(TEntity entity);
    Task<bool> UpdateAsync(TEntity entity);
    Task<IDbContextTransaction> BeginTransactionAsync();
    Task CommitAsync(IDbContextTransaction transaction);
    Task<bool?> HardDeleteAsync(TEntity entity);
}