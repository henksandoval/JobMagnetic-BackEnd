using JobMagnet.Domain.Core.Entities;
using JobMagnet.Domain.Ports.Repositories.Base;

namespace JobMagnet.Domain.Ports.Repositories;

public interface ISummaryQueryRepository : IQueryRepository<ProfessionalSummary, long>
{
    Task<IReadOnlyCollection<ProfessionalSummary>> GetAllWithIncludesAsync();
    Task<ProfessionalSummary?> GetByIdWithIncludesAsync(long id);
    ISummaryQueryRepository IncludeWorkExperience();
    ISummaryQueryRepository IncludeEducation();
}