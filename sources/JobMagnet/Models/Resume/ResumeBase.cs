namespace JobMagnet.Models.Resume;

public abstract class ResumeBase
{
    public required string JobTitle { get; set; }
    public DateOnly? BirthDate { get; set; }
    public required string About { get; set; }
    public required string Summary { get; set; }
    public required string Overview { get; set; }
    public required string ProfileImageUrl { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public string? Title { get; set; }
    public string? Suffix { get; set; }
    public string? MiddleName { get; set; }
    public string? SecondLastName { get; set; }
}