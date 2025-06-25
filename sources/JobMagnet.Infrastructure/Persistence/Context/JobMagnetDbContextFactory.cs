using JobMagnet.Application.Services;
using Microsoft.EntityFrameworkCore;

namespace JobMagnet.Infrastructure.Persistence.Context;

public interface IJobMagnetDbContextFactory
{
    JobMagnetDbContext CreateDbContext();
}

public class JobMagnetJobMagnetDbContextFactory(
    DbContextOptions<JobMagnetDbContext> options,
    ICurrentUserService currentUserService)
    : IJobMagnetDbContextFactory
{
    public JobMagnetDbContext CreateDbContext() => new(options, currentUserService);
}