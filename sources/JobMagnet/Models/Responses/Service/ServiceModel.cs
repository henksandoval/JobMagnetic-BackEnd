using JobMagnet.Models.Base;

namespace JobMagnet.Models.Responses.Service;

public sealed class ServiceModel : ServiceBase, IIdentifierBase<int>
{
    public required int Id { get; init; }
}