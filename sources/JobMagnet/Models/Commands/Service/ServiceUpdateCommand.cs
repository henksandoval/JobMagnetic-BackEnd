using JobMagnet.Models.Base;

namespace JobMagnet.Models.Commands.Service;

public sealed class ServiceUpdateCommand
{
    public long? Id { get; init; }
    public ServiceBase ServiceData { get; init; }
}