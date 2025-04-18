using System.Linq.Expressions;
using JobMagnet.Infrastructure.Entities;
using JobMagnet.Infrastructure.Repositories.Base.Interfaces;

namespace JobMagnet.Infrastructure.Repositories.Interfaces;

public interface IProfileQueryRepository : IQueryRepository<ProfileEntity, long>
{
    Task<ProfileEntity?> GetFirstByExpressionWithIncludesAsync(Expression<Func<ProfileEntity, bool>> expression);
    IProfileQueryRepository IncludeResume();
    IProfileQueryRepository IncludeTalents();
    IProfileQueryRepository IncludeTestimonials();
}