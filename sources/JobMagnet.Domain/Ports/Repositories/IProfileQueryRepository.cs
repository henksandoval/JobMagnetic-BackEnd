using System.Linq.Expressions;
using JobMagnet.Domain.Aggregates.Profiles;
using JobMagnet.Domain.Ports.Repositories.Base;

namespace JobMagnet.Domain.Ports.Repositories;

public interface IProfileQueryRepository : IQueryRepository<Profile, ProfileId>
{
    IProfileQueryRepository WhereCondition(Expression<Func<Profile, bool>> expression);
    IProfileQueryRepository WithResume();
    IProfileQueryRepository WithSkills();
    IProfileQueryRepository WithTalents();
    IProfileQueryRepository WithProject();
    IProfileQueryRepository WithSummary();
    IProfileQueryRepository WithTestimonials();
    Task<Profile?> BuildFirstOrDefaultAsync();
}