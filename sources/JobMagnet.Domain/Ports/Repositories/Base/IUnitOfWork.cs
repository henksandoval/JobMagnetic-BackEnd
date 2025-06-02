using JobMagnet.Domain.Core.Entities;

namespace JobMagnet.Domain.Ports.Repositories.Base;

public interface IUnitOfWork : IAsyncDisposable
{
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    ICommandRepository<ProfileEntity> ProfileRepository { get; }
    ICommandRepository<ResumeEntity> ResumeRepository { get; }
    ICommandRepository<SkillEntity> SkillRepository { get; }
    ICommandRepository<ServiceEntity> ServiceRepository { get; }
    ICommandRepository<SummaryEntity> SummaryRepository { get; }
    ICommandRepository<TalentEntity> TalentRepository { get; }
    ICommandRepository<PortfolioGalleryEntity> PortfolioGalleryRepository { get; }
    ICommandRepository<TestimonialEntity> TestimonialRepository { get; }
}