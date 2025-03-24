using JobMagnet.Models.Shared;

namespace JobMagnet.Models.About;

public class AboutUpdateRequest : AboutBase, IIdentifierBase<int>
{
    public int Id { get; init; }
}