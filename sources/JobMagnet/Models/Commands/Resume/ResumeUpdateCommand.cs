using JobMagnet.Models.Base;

namespace JobMagnet.Models.Commands.Resume;

public sealed class ResumeUpdateCommand : ResumeBase, IIdentifierBase<long>
{
    public required long Id { get; init; }
}