using JobMagnet.Domain.Ports.Repositories.Base;
using JobMagnet.Infrastructure.Persistence.Context;

namespace JobMagnet.Infrastructure.Persistence.Repositories.Base;

public class UnitOfWork(JobMagnetDbContext dbContext) : IUnitOfWork
{
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
        dbContext.SaveChangesAsync(cancellationToken);
}