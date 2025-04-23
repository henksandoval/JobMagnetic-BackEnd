using System.Linq.Expressions;
using JobMagnet.Infrastructure.Entities;
using JobMagnet.Infrastructure.Repositories.Base.Interfaces;

namespace JobMagnet.Infrastructure.Repositories.Interfaces;

public interface IProfileQueryRepository : IQueryRepository<ProfileEntity, long>
{
    IProfileQueryRepository WhereCondition(Expression<Func<ProfileEntity, bool>> expression);
    Task<ProfileEntity?> BuildAsync();
    IProfileQueryRepository WithResume();
    IProfileQueryRepository WithSkills();
    IProfileQueryRepository WithTalents();
    IProfileQueryRepository WithPortfolioGallery();
    IProfileQueryRepository WithSummaries();
    IProfileQueryRepository WithServices();
    IProfileQueryRepository WithTestimonials();
}