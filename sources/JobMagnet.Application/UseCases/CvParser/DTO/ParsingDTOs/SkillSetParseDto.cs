namespace JobMagnet.Application.UseCases.CvParser.DTO.ParsingDTOs;

public class SkillSetParseDto
{
    public List<SkillParseDto> Skills { get; set; }
    public string? Overview { get; set; }
}