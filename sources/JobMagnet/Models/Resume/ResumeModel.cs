using JobMagnet.Models.Base;

namespace JobMagnet.Models.Resume;

public sealed class ResumeModel : ResumeBase, IIdentifierBase<int>
{
    public required int Id { get; init; }
}