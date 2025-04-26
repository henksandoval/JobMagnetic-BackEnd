using JobMagnet.Models.Base;

namespace JobMagnet.Models.Commands.Service;

public sealed class ServiceCommand : ServiceBase
{
    public long? Id { get; init; }
}