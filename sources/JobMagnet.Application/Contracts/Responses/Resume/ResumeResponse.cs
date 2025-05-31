using JobMagnet.Application.Contracts.Responses.Base;

namespace JobMagnet.Application.Contracts.Responses.Resume;

public sealed record ResumeResponse : IIdentifierBase<long>
{
    public required ResumeBase ResumeData { get; init; }
    public required long Id { get; init; }
}