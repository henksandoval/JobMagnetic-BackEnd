using JobMagnet.Application.Contracts.Responses.Base;

namespace JobMagnet.Application.Contracts.Commands.Project;

public sealed record ProjectCommand
{
    public ProjectBase? ProjectData { get; init; }
}