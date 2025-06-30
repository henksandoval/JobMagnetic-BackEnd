using JobMagnet.Application.Contracts.Responses.Base;

namespace JobMagnet.Application.Contracts.Commands.Talent;

public sealed record TalentCommand
{
    public TalentBase? TalentData  { get; init; }
}