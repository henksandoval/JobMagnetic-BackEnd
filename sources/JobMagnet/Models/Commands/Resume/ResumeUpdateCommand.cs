using JobMagnet.Models.Base;

namespace JobMagnet.Models.Commands.Resume;

public sealed record ResumeUpdateCommand : IIdentifierBase<long>
{
    public required long Id { get; init; }
    public required ResumeBase ResumeData { get; init; }
}