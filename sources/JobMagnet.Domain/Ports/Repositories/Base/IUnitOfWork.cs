using JobMagnet.Domain.Core.Entities;
using JobMagnet.Domain.Core.Entities.Skills;

namespace JobMagnet.Domain.Ports.Repositories.Base;

public interface IUnitOfWork
{
    ICommandRepository<Profile> ProfileRepository { get; }
    ICommandRepository<VanityUrl> PublicProfileIdentifierRepository { get; }
    ICommandRepository<Headline> ResumeRepository { get; }
    ICommandRepository<SkillSet> SkillRepository { get; }
    ICommandRepository<CareerHistory> SummaryRepository { get; }
    ICommandRepository<Talent> TalentRepository { get; }
    ICommandRepository<Project> ProjectRepository { get; }
    ICommandRepository<Testimonial> TestimonialRepository { get; }
    Task ExecuteOperationInTransactionAsync(Func<Task> operation, CancellationToken cancellationToken = default);
}