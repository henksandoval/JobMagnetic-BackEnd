namespace JobMagnet.Models.Base;

public record ResumeQueryBase
{
    public string? JobTitle { get; set; }
    public long ProfileId { get; set; }
    public DateOnly? BirthDate { get; set; }
    public string? About { get; set; }
    public string? Summary { get; set; }
    public string? Overview { get; set; }
    public string? ProfileImageUrl { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Title { get; set; }
    public string? Suffix { get; set; }
    public string? MiddleName { get; set; }
    public string? SecondLastName { get; set; }
}