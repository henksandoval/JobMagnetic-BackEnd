namespace JobMagnet.Models.Profile;

public class ProfileModel
{
    public required string ProfileImageUrl { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public DateOnly? BirthDate { get; set; }
    public string? MiddleName { get; set; }
    public string? SecondLastName { get; set; }
}