namespace JobMagnet.Models.Base;

public record ResumeCommandBase
{
    public long ProfileId { get; set; }
    public string? JobTitle { get; set; }
    public string? About { get; set; }
    public string? Summary { get; set; }
    public string? Overview { get; set; }
    public string? Title { get; set; }
    public string? Suffix { get; set; }
    public string? Address { get; set; }
}