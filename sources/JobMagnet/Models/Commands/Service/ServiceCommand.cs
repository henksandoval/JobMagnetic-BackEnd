using JobMagnet.Models.Base;

namespace JobMagnet.Models.Commands.Service;

public sealed record ServiceCommand
{
    public required ServiceBase ServiceData { get; init; }
}