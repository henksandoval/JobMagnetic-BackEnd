using JobMagnet.Models.Base;

namespace JobMagnet.Models.Service;

public sealed class ServiceRequest : ServiceBase
{
    public long? Id { get; init; }
}