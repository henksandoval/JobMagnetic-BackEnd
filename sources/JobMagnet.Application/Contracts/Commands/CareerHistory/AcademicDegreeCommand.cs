using JobMagnet.Application.Contracts.Responses.Base;

namespace JobMagnet.Application.Contracts.Commands.CareerHistory;

public sealed record AcademicDegreeCommand
{
    public required AcademicDegreeBase AcademicDegreeData { get; init; }
}