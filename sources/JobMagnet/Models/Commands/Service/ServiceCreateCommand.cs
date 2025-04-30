using JobMagnet.Models.Base;

namespace JobMagnet.Models.Commands.Service;

public sealed class ServiceCreateCommand
{
    public ServiceBase ServiceData { get; init; }
}