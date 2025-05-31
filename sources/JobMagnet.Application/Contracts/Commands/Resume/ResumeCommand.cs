using JobMagnet.Application.Contracts.Responses.Base;

namespace JobMagnet.Application.Contracts.Commands.Resume;

public sealed record ResumeCommand
{
    public required ResumeCommandBase ResumeData { get; init; }
}