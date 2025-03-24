using JobMagnet.Models.Shared;

namespace JobMagnet.Models.About;

public sealed class AboutModel : AboutBase, IIdentifierBase<int>
{
    public int Id { get; init; }
}