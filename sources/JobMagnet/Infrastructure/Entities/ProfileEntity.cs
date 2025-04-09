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

    public virtual ICollection<ResumeEntity> Resumes { get; set; }
}