using JobMagnet.Infrastructure.Entities.Base;

namespace JobMagnet.Infrastructure.Entities;

// ReSharper disable once ClassNeverInstantiated.Global
public class ResumeEntity : SoftDeletableEntity<long>
{
    public string JobTitle { get; set; }
    public DateOnly? BirthDate { get; set; }
    public string About { get; set; }
    public string Summary { get; set; }
    public string Overview { get; set; }
    public string ProfileImageUrl { get; set; }

    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public string? Title { get; set; }
    public string? Suffix { get; set; }
    public string? MiddleName { get; set; }
    public string? SecondLastName { get; set; }
}