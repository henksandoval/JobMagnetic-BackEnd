namespace JobMagnet.Domain.Core.Services.CvParser.Interfaces;

public interface IParsedSkillDetail
{
    string? Name { get; }
    ushort? Level { get; }
}