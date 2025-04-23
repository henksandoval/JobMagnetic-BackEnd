namespace JobMagnet.Infrastructure.DTO.Profile;

// ReSharper disable once ClassNeverInstantiated.Global
public class ProfileDTO
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public string ProfileImageUrl { get; set; }
    public DateOnly? BirthDate { get; set; }
    public string? MiddleName { get; set; }
    public string? SecondLastName { get; set; }

    public virtual ResumeDTO Resume { get; set; }
    public virtual SkillDTO Skill { get; set; }
    public virtual ServiceDTO Services { get; set; }
    public virtual ICollection<TalentDTO> Talents { get; set; }
    public virtual ICollection<PortfolioGalleryDTO> PortfolioGallery { get; set; }
    public virtual ICollection<SummaryDTO> Summaries { get; set; }
    public virtual ICollection<TestimonialDTO> Testimonials { get; set; }
}