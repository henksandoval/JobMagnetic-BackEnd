using JobMagnet.Entities;
using Microsoft.EntityFrameworkCore;

namespace JobMagnet.Context;

public class JobMagnetDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<AboutEntity> About { get; set; }
    public DbSet<SkillEntity> Skill { get; set; }
}