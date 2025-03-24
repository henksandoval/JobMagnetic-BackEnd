namespace JobMagnet.Models.About;

public abstract class AboutBase
{
    public required string Description { get; init; }
    public required string ImageUrl { get; init; }
    public required string Text { get; init; }
    public required string Hobbies { get; init; }
    public required string Birthday { get; init; }
    public required string WebSite { get; init; }
    public required int PhoneNumber { get; init; }
    public required string City { get; init; }
    public required int Age { get; init; }
    public required string Degree { get; init; }
    public required string Email { get; init; }
    public required string Freelance { get; init; }
    public required string WorkExperience { get; init; }
}