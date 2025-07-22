namespace JobMagnet.Shared.Abstractions;

public interface IGuidGenerator
{
    Guid NewGuid();
}

public class GuidGenerator : IGuidGenerator
{
    public Guid NewGuid() => Guid.NewGuid();
}