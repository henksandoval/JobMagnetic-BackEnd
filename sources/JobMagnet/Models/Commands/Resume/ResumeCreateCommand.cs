using JobMagnet.Models.Base;

namespace JobMagnet.Models.Commands.Resume;

public sealed record ResumeCreateCommand
{
    public required ResumeCommandBase ResumeQueryData { get; init; }
}