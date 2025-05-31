using JobMagnet.Application.Contracts.Responses.Base;

namespace JobMagnet.Application.Contracts.Commands.Service;

public sealed record ServiceCommand
{
    public required ServiceBase ServiceData { get; init; }
}