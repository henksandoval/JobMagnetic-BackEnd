using System.Linq.Expressions;
using JobMagnet.Domain.Core.Entities;
using JobMagnet.Domain.Ports.Repositories.Base;

namespace JobMagnet.Domain.Ports.Repositories;

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