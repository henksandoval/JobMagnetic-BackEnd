using JobMagnet.Models.Base;

namespace JobMagnet.Models.Commands.Resume;

public sealed class ResumeUpdateRequest : ResumeBase, IIdentifierBase<long>
{
    public required long Id { get; init; }
}