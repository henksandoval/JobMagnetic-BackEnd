using JobMagnet.Models.Base;

namespace JobMagnet.Models.Commands.Service;

public sealed class ServiceCreateCommand : ServiceBase
{
    public ServiceBase ServiceData { get; init; }
}