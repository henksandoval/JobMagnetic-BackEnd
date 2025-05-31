namespace JobMagnet.Application.UseCases.CvParser.RawDTOs;

public class ResumeRaw
{
    public string? JobTitle { get; set; }
    public string? About { get; set; }
    public string? Summary { get; set; }
    public string? Overview { get; set; }
    public string? Title { get; set; }
    public string? Suffix { get; set; }
    public string? Address { get; set; }
    public IEnumerable<ContactInfoRaw> ContactInfo { get; set; }
}