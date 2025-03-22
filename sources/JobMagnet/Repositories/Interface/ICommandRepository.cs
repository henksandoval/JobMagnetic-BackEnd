using Microsoft.EntityFrameworkCore.Storage;

namespace JobMagnet.Repositories.Interface;

public interface ICommandRepository<TEntity> where TEntity : class
{
    Task<TEntity?> GetByIdAsync(int id);
    Task CreateAsync(TEntity entity);
    Task<bool> UpdateAsync(TEntity entity);
    Task<IDbContextTransaction> BeginTransactionAsync();
    Task CommitAsync(IDbContextTransaction transaction);
}