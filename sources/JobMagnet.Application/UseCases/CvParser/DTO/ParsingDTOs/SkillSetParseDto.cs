namespace JobMagnet.Application.UseCases.CvParser.DTO.ParsingDTOs;

public class SkillSetParseDto
{
    public IEnumerable<SkillParseDto> Skills { get; set; }
    public string? Overview { get; set; }
}