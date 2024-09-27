using JobMagnet.Entities;
using Microsoft.EntityFrameworkCore;

namespace JobMagnet.Context
{
    public class JobMagnetDbContext : DbContext
    {
        public DbSet<AboutEntity> About { get; set; }
        public JobMagnetDbContext(DbContextOptions options) : base (options)
        {
        }
    }
}
