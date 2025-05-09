namespace JobMagnet.Models.Base;

public sealed record ResumeQueryBase
{
    public long ProfileId { get; init; }
    public string? JobTitle { get; init; }
    public DateOnly? BirthDate { get; init; }
    public string? About { get; init; }
    public string? Summary { get; init; }
    public string? Overview { get; init; }
    public string? ProfileImageUrl { get; init; }
    public string? FirstName { get; init; }
    public string? LastName { get; init; }
    public string? Title { get; init; }
    public string? Suffix { get; init; }
    public string? MiddleName { get; init; }
    public string? SecondLastName { get; init; }
}