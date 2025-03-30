using JobMagnet.Models.Shared;

namespace JobMagnet.Models.Service;

public sealed class ServiceModel : ServiceBase, IIdentifierBase<int>
{
    public required int Id { get; init; }
}