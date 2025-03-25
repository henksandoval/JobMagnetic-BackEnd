using JobMagnet.Models.Shared;

namespace JobMagnet.Models.Resume;

public class ResumeUpdateRequest : ResumeBase, IIdentifierBase<long>
{
    public required long Id { get; init; }
}