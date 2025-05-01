using JobMagnet.Models.Base;

namespace JobMagnet.Models.Commands.Service;

public sealed class ServiceUpdateCommand : ServiceBase
{
    public long? Id { get; init; }
}