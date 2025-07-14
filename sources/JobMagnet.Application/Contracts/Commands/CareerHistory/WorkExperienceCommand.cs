using JobMagnet.Application.Contracts.Responses.Base;

namespace JobMagnet.Application.Contracts.Commands.CareerHistory;

public sealed record WorkExperienceCommand
{
    public required WorkExperienceBase WorkExperienceData { get; init; }
}