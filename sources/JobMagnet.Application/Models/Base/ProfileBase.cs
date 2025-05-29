namespace JobMagnet.Application.Models.Base;

public sealed record ProfileBase
{
    public string? FirstName { get; init; }
    public string? LastName { get; init; }
    public string? ProfileImageUrl { get; init; }
    public DateOnly? BirthDate { get; init; }
    public string? MiddleName { get; init; }
    public string? SecondLastName { get; init; }
}