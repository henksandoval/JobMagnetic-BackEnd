using JobMagnet.Domain.Core.Entities;

namespace JobMagnet.Domain.Ports.Repositories.Base;

public interface IUnitOfWork
{
    Task ExecuteOperationInTransactionAsync(Func<Task> operation, CancellationToken cancellationToken = default);

    ICommandRepository<ProfileEntity> ProfileRepository { get; }
    ICommandRepository<PublicProfileIdentifierEntity> PublicProfileIdentifierRepository { get; }
    ICommandRepository<ResumeEntity> ResumeRepository { get; }
    ICommandRepository<SkillEntity> SkillRepository { get; }
    ICommandRepository<ServiceEntity> ServiceRepository { get; }
    ICommandRepository<SummaryEntity> SummaryRepository { get; }
    ICommandRepository<TalentEntity> TalentRepository { get; }
    ICommandRepository<PortfolioGalleryEntity> PortfolioGalleryRepository { get; }
    ICommandRepository<TestimonialEntity> TestimonialRepository { get; }
}