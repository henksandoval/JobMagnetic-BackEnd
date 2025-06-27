using JobMagnet.Shared.Abstractions;

namespace JobMagnet.Infrastructure.Persistence.Seeders;

public class SeederSequentialGuidGenerator : IGuidGenerator
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