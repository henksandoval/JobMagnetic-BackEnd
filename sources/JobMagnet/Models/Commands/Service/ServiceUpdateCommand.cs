using JobMagnet.Models.Base;

namespace JobMagnet.Models.Commands.Service;

public sealed class ServiceUpdateCommand : IIdentifierBase<long>
{
    public required long Id { get; init; }
    public required ServiceBase ServiceData { get; init; }
}