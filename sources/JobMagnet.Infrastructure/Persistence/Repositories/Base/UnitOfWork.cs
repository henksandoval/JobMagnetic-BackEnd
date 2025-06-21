using JobMagnet.Domain.Core.Entities;
using JobMagnet.Domain.Core.Entities.Skills;
using JobMagnet.Domain.Ports.Repositories.Base;
using JobMagnet.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace JobMagnet.Infrastructure.Persistence.Repositories.Base;

public class UnitOfWork(JobMagnetDbContext dbContext, ILogger<UnitOfWork> logger) : IUnitOfWork
{
    private readonly JobMagnetDbContext _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    private readonly ILogger<UnitOfWork> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    private ICommandRepository<ProfileEntity>? _profileRepository;
    private ICommandRepository<PublicProfileIdentifierEntity>? _publicProfileIdentifierRepository;
    private ICommandRepository<ResumeEntity>? _resumeRepository;
    private ICommandRepository<SkillSetEntity>? _skillRepository;
    private ICommandRepository<ServiceEntity>? _serviceRepository;
    private ICommandRepository<SummaryEntity>? _summaryRepository;
    private ICommandRepository<TalentEntity>? _talentRepository;
    private ICommandRepository<PortfolioGalleryEntity>? _portfolioGalleryRepository;
    private ICommandRepository<TestimonialEntity>? _testimonialRepository;

    public ICommandRepository<ProfileEntity> ProfileRepository =>
        _profileRepository ??= new Repository<ProfileEntity, long>(_dbContext);

    public ICommandRepository<PublicProfileIdentifierEntity> PublicProfileIdentifierRepository =>
        _publicProfileIdentifierRepository ??= new Repository<PublicProfileIdentifierEntity, long>(_dbContext);
    public ICommandRepository<ResumeEntity> ResumeRepository =>
        _resumeRepository ??= new Repository<ResumeEntity, long>(_dbContext);
    public ICommandRepository<SkillSetEntity> SkillRepository =>
        _skillRepository ??= new Repository<SkillSetEntity, long>(_dbContext);
    public ICommandRepository<ServiceEntity> ServiceRepository =>
        _serviceRepository ??= new Repository<ServiceEntity, long>(_dbContext);
    public ICommandRepository<SummaryEntity> SummaryRepository =>
        _summaryRepository ??= new Repository<SummaryEntity, long>(_dbContext);
    public ICommandRepository<TalentEntity> TalentRepository =>
        _talentRepository ??= new Repository<TalentEntity, long>(_dbContext);
    public ICommandRepository<PortfolioGalleryEntity> PortfolioGalleryRepository =>
        _portfolioGalleryRepository ??= new Repository<PortfolioGalleryEntity, long>(_dbContext);
    public ICommandRepository<TestimonialEntity> TestimonialRepository =>
        _testimonialRepository ??= new Repository<TestimonialEntity, long>(_dbContext);

    public async Task ExecuteOperationInTransactionAsync(Func<Task> operation, CancellationToken cancellationToken = default)
    {
        var strategy = _dbContext.Database.CreateExecutionStrategy();

        await strategy.ExecuteAsync(async () =>
        {
            await using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
            _logger.LogInformation("Transaction started with ID: {TransactionId}", transaction.TransactionId);

            try
            {
                await operation();

                await _dbContext.SaveChangesAsync(cancellationToken);
                _logger.LogInformation("Changes saved successfully within transaction {TransactionId}.", transaction.TransactionId);

                await transaction.CommitAsync(cancellationToken);
                _logger.LogInformation("Transaction {TransactionId} committed successfully.", transaction.TransactionId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during transactional operation for transaction {TransactionId}. Rolling back.", transaction.TransactionId);
                await transaction.RollbackAsync(cancellationToken);
                _logger.LogInformation("Transaction {TransactionId} rolled back.", transaction.TransactionId);
                throw;
            }
        });
    }
}