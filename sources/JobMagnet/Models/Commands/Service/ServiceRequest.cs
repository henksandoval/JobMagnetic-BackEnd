using JobMagnet.Models.Base;

namespace JobMagnet.Models.Commands.Service;

public sealed class ServiceRequest : ServiceBase
{
    public long? Id { get; init; }
}