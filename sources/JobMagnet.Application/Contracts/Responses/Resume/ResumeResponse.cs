using JobMagnet.Application.Contracts.Responses.Base;

namespace JobMagnet.Application.Contracts.Responses.Resume;

public sealed record ResumeResponse : IIdentifierBase<Guid>
{
    public required ResumeBase ResumeData { get; init; }
    public required Guid Id { get; init; }
}