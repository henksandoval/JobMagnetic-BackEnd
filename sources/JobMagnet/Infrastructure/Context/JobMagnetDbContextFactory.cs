using Microsoft.EntityFrameworkCore;

namespace JobMagnet.Infrastructure.Context;

public interface IDbContextFactory
{
    JobMagnetDbContext CreateDbContext();
}

public class JobMagnetDbContextFactory(DbContextOptions<JobMagnetDbContext> options, IHttpContextAccessor? httpContextAccessor)
    : IDbContextFactory
{
    public JobMagnetDbContext CreateDbContext()
    {
        return new JobMagnetDbContext(options, httpContextAccessor);
    }
}