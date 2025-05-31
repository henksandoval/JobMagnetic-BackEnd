namespace JobMagnet.Domain.Domain.Services.CvParser.Interfaces;

public interface IParsedSkillDetail
{
    string? Name { get; }
    ushort? Level { get; }
}