using JobMagnet.Models.Base;

namespace JobMagnet.Models.Commands.Resume;

public sealed record ResumeCommand
{
    public required ResumeCommandBase ResumeData { get; init; }
}