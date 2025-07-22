using JobMagnet.Shared.Abstractions;

namespace JobMagnet.Shared.Tests.Abstractions;

public class StubGuidGenerator(Guid guid) : IGuidGenerator
{
    public Guid NewGuid() => guid;
}