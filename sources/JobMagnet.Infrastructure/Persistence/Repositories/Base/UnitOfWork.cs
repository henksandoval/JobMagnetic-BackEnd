using JobMagnet.Domain.Core.Entities;
using JobMagnet.Domain.Ports.Repositories.Base;
using JobMagnet.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;

namespace JobMagnet.Infrastructure.Persistence.Repositories.Base;

public class UnitOfWork(JobMagnetDbContext dbContext, ILogger<UnitOfWork> logger) : IUnitOfWork
{
    private readonly JobMagnetDbContext _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    private IDbContextTransaction? _currentTransaction;

    private ICommandRepository<ProfileEntity>? _profileRepository;
    private ICommandRepository<ResumeEntity>? _resumeRepository;
    private ICommandRepository<SkillEntity>? _skillRepository;
    private ICommandRepository<ServiceEntity>? _serviceRepository;
    private ICommandRepository<SummaryEntity>? _summaryRepository;
    private ICommandRepository<TalentEntity>? _talentRepository;
    private ICommandRepository<PortfolioGalleryEntity>? _portfolioGalleryRepository;
    private ICommandRepository<TestimonialEntity>? _testimonialRepository;

    public ICommandRepository<ProfileEntity> ProfileRepository =>
        _profileRepository ??= new Repository<ProfileEntity, long>(_dbContext);
    public ICommandRepository<ResumeEntity> ResumeRepository =>
        _resumeRepository ??= new Repository<ResumeEntity, long>(_dbContext);
    public ICommandRepository<SkillEntity> SkillRepository =>
        _skillRepository ??= new Repository<SkillEntity, long>(_dbContext);
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

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_currentTransaction != null)
        {
            return;
        }
        _currentTransaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            if (_currentTransaction == null)
                throw new InvalidOperationException("Transaction has not been started.");

            await SaveChangesAsync(cancellationToken);
            await _currentTransaction.CommitAsync(cancellationToken);
        }
        catch
        {
            await RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            if (_currentTransaction != null)
            {
                await _currentTransaction.RollbackAsync(cancellationToken);
            }
        }
        catch
        {
            logger.LogCritical("There was an error while rolling back the transaction.");
        }
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async ValueTask DisposeAsync()
    {
        if (_currentTransaction != null)
        {
            await _currentTransaction.DisposeAsync();
            _currentTransaction = null;
        }
    }
}