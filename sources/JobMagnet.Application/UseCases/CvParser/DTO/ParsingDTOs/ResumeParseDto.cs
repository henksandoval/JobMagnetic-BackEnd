namespace JobMagnet.Application.UseCases.CvParser.DTO.ParsingDTOs;

public class ResumeParseDto
{
    public List<ContactInfoParseDto> ContactInfo { get; set; }
    public string? JobTitle { get; set; }
    public string? About { get; set; }
    public string? Summary { get; set; }
    public string? Overview { get; set; }
    public string? Title { get; set; }
    public string? Suffix { get; set; }
    public string? Address { get; set; }
}