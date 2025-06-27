using JobMagnet.Shared.Abstractions;

namespace JobMagnet.Shared.Tests.Abstractions;

public class SequentialGuidGenerator : IGuidGenerator
{
    private int _counter = 0;
    public Guid NewGuid()
    {
        _counter++;
        var bytes = new byte[16];
        BitConverter.GetBytes(_counter).CopyTo(bytes, 0);
        return new Guid(bytes);
    }
}