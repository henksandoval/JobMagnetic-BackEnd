using JobMagnet.Shared.Abstractions;

namespace JobMagnet.Infrastructure.Persistence.Seeders;

public class SeederSequentialGuidGenerator : IGuidGenerator
{
    private int _counter;

    public Guid NewGuid()
    {
        _counter++;
        var bytes = new byte[16];
        BitConverter.GetBytes(_counter).CopyTo(bytes, 0);
        return new Guid(bytes);
    }

    public static Guid FromInt(int id)
    {
        var bytes = new byte[16];
        var intBytes = BitConverter.GetBytes(id);
        intBytes.CopyTo(bytes, 0);
        return new Guid(bytes);
    }
}