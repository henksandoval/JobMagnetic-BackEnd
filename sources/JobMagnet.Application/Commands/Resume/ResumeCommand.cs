using JobMagnet.Application.Models.Base;

namespace JobMagnet.Application.Commands.Resume;

public sealed record ResumeCommand
{
    public required ResumeCommandBase ResumeData { get; init; }
}