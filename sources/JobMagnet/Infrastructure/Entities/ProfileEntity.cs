using System.ComponentModel.DataAnnotations.Schema;
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

    public virtual ResumeEntity Resume { get; set; }
    public virtual ICollection<TalentEntity> Talents { get; set; }
    public virtual ICollection<PortfolioEntity> Portfolios { get; set; }
    public virtual ICollection<SummaryEntity> Summaries { get; set; }
    public virtual ICollection<ServiceEntity> Services { get; set; }
    public virtual ICollection<SkillEntity> Skills { get; set; }
    public virtual ICollection<TestimonialEntity> Testimonials { get; set; }
}

public class ContactInfoEntity : SoftDeletableEntity<long>
{
    public required string Value { get; set; }

    [ForeignKey(nameof(ContactType))] public long ContactTypeId { get; set; }
    [ForeignKey(nameof(Resume))] public long ResumeId { get; set; }

    public virtual ContactTypeEntity ContactType { get; set; }
    public virtual ResumeEntity Resume { get; set; }
}

public class ContactTypeEntity : SoftDeletableEntity<int>
{
    /*
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Website { get; set; }
    public string? SocialMedia { get; set; }
    public string? LinkedIn { get; set; }
    public string? GitHub { get; set; }
    public string? Twitter { get; set; }
    public string? Facebook { get; set; }
    public string? Instagram { get; set; }
     */
    public required string Name { get; set; }
    public string? IconClass { get; set; }
    public string? IconUrl { get; set; }

    public virtual ICollection<ContactInfoEntity> ContactDetails { get; set; }
}