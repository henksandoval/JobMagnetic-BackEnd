namespace JobMagnet.Application.UseCases.CvParser.DTO.ParsingDTOs;

public class SkillParseDto
{
    public IEnumerable<SkillDetailParseDto> SkillDetails { get; set; }
    public string? Overview { get; set; }
}