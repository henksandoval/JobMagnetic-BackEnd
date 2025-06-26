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

    private ICommandRepository<Profile>? _profileRepository;
    private ICommandRepository<Project>? _projectRepository;
    private ICommandRepository<VanityUrl>? _publicProfileIdentifierRepository;
    private ICommandRepository<Resume>? _resumeRepository;
    private ICommandRepository<SkillSet>? _skillRepository;
    private ICommandRepository<ProfessionalSummary>? _summaryRepository;
    private ICommandRepository<Talent>? _talentRepository;
    private ICommandRepository<Testimonial>? _testimonialRepository;

    public ICommandRepository<Profile> ProfileRepository =>
        _profileRepository ??= new Repository<Profile, long>(_dbContext);

    public ICommandRepository<VanityUrl> PublicProfileIdentifierRepository =>
        _publicProfileIdentifierRepository ??= new Repository<VanityUrl, long>(_dbContext);

    public ICommandRepository<Resume> ResumeRepository =>
        _resumeRepository ??= new Repository<Resume, long>(_dbContext);

    public ICommandRepository<SkillSet> SkillRepository =>
        _skillRepository ??= new Repository<SkillSet, long>(_dbContext);

    public ICommandRepository<ProfessionalSummary> SummaryRepository =>
        _summaryRepository ??= new Repository<ProfessionalSummary, long>(_dbContext);

    public ICommandRepository<Talent> TalentRepository =>
        _talentRepository ??= new Repository<Talent, long>(_dbContext);

    public ICommandRepository<Project> ProjectRepository =>
        _projectRepository ??= new Repository<Project, long>(_dbContext);

    public ICommandRepository<Testimonial> TestimonialRepository =>
        _testimonialRepository ??= new Repository<Testimonial, long>(_dbContext);

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
                _logger.LogError(ex, "Error occurred during transactional operation for transaction {TransactionId}. Rolling back.",
                    transaction.TransactionId);
                await transaction.RollbackAsync(cancellationToken);
                _logger.LogInformation("Transaction {TransactionId} rolled back.", transaction.TransactionId);
                throw;
            }
        });
    }
}