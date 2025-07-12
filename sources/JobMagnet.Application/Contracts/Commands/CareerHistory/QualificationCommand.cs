using JobMagnet.Application.Contracts.Responses.Base;

namespace JobMagnet.Application.Contracts.Commands.CareerHistory;

public sealed record QualificationCommand
{
    public required QualificationBase QualificationData { get; init; }
}