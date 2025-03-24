using JobMagnet.Models.Shared;

namespace JobMagnet.Models.About;

public class AboutUpdateRequest : AboutBase, IIdentifierBase<int>
{
    public required int Id { get; init; }
}