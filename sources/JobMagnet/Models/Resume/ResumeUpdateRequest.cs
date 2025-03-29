using JobMagnet.Models.Shared;

namespace JobMagnet.Models.Resume;

public sealed class ResumeUpdateRequest : ResumeBase, IIdentifierBase<long>
{
    public required long Id { get; init; }
}