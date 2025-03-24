using Microsoft.EntityFrameworkCore.Storage;

namespace JobMagnet.Repositories.Interfaces;

public interface ICommandRepository<in TEntity> where TEntity : class
{
    Task CreateAsync(TEntity entity);
    Task<bool> UpdateAsync(TEntity entity);
    Task<IDbContextTransaction> BeginTransactionAsync();
    Task CommitAsync(IDbContextTransaction transaction);
    Task<bool?> HardDeleteAsync(TEntity entity);
}