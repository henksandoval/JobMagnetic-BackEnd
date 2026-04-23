using JobMagnet.Application.Services;
using Microsoft.EntityFrameworkCore;

namespace JobMagnet.Infrastructure.Persistence.Context;

public interface IJobMagnetDbContextFactory
{
    JobMagnetDbContext CreateDbContext();
}

public class JobMagnetDbContextFactory(DbContextOptions<JobMagnetDbContext> options)
    : IJobMagnetDbContextFactory
{
    public JobMagnetDbContext CreateDbContext() => new(options);
}