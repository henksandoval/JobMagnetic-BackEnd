using JobMagnet.Models.Base;

namespace JobMagnet.Models.Commands.Service;

public sealed record ServiceCreateCommand
{
    public required ServiceBase ServiceData { get; init; }
}