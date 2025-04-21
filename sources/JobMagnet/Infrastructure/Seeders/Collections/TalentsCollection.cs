using JobMagnet.Infrastructure.Entities;

namespace JobMagnet.Infrastructure.Seeders.Collections;

// ReSharper disable once NotAccessedPositionalProperty.Global
public record TalentsCollection(long ProfileId = 0)
{
    public readonly IList<TalentEntity> Talents = new List<TalentEntity>
    {
        new()
        {
            Id = 0,
            Description = "Creative",
            ProfileId = ProfileId,
            AddedAt = DateTime.Now,
            AddedBy = Guid.Empty
        },
        new()
        {
            Id = 0,
            Description = "Problem Solver",
            ProfileId = ProfileId,
            AddedAt = DateTime.Now,
            AddedBy = Guid.Empty
        },
        new()
        {
            Id = 0,
            Description = "Team Player",
            ProfileId = ProfileId,
            AddedAt = DateTime.Now,
            AddedBy = Guid.Empty
        },
        new()
        {
            Id = 0,
            Description = "Fast Learner",
            ProfileId = ProfileId,
            AddedAt = DateTime.Now,
            AddedBy = Guid.Empty
        },
    };
}