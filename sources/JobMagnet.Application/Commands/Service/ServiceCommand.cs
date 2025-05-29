using JobMagnet.Application.Models.Base;

namespace JobMagnet.Application.Commands.Service;

public sealed record ServiceCommand
{
    public required ServiceBase ServiceData { get; init; }
}