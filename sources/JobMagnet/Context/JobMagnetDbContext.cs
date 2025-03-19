using JobMagnet.Entities;
using Microsoft.EntityFrameworkCore;

namespace JobMagnet.Context;

public class JobMagnetDbContext : DbContext
{
    public JobMagnetDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<AboutEntity> About { get; set; }
}