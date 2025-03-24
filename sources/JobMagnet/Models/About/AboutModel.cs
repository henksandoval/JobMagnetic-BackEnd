using JobMagnet.Models.Shared;

namespace JobMagnet.Models.About;

public sealed class AboutModel : AboutBase, IIdentifierBase<int>
{
    public required int Id { get; init; }
}