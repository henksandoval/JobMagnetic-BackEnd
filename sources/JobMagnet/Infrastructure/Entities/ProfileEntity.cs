using JobMagnet.Infrastructure.Entities.Base;

namespace JobMagnet.Infrastructure.Entities;

// ReSharper disable once ClassNeverInstantiated.Global
public class ProfileEntity : SoftDeletableEntity<long>
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public string ProfileImageUrl { get; set; }
    public DateOnly? BirthDate { get; set; }
    public string? MiddleName { get; set; }
    public string? SecondLastName { get; set; }

    public virtual ICollection<TalentEntity> Talents { get; set; }
    public virtual ICollection<ResumeEntity> Resumes { get; set; }
    public virtual ICollection<PortfolioEntity> Portfolios { get; set; }
    public virtual ICollection<SummaryEntity> Summaries { get; set; }
    public virtual ICollection<ServiceEntity> Services { get; set; }
    public virtual ICollection<SkillEntity> Skills { get; set; }
    public virtual ICollection<TestimonialEntity> Testimonials { get; set; }
}