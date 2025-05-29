using System.Linq.Expressions;
using JobMagnet.Domain.Domain.Ports.Repositories.Base;
using JobMagnet.Domain.Entities;

namespace JobMagnet.Domain.Domain.Ports.Repositories;

public interface IProfileQueryRepository : IQueryRepository<ProfileEntity, long>
{
    IProfileQueryRepository WhereCondition(Expression<Func<ProfileEntity, bool>> expression);
    IProfileQueryRepository WithResume();
    IProfileQueryRepository WithSkills();
    IProfileQueryRepository WithTalents();
    IProfileQueryRepository WithPortfolioGallery();
    IProfileQueryRepository WithSummaries();
    IProfileQueryRepository WithServices();
    IProfileQueryRepository WithTestimonials();
    Task<ProfileEntity?> BuildFirstOrDefaultAsync();
}