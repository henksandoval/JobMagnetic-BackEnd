namespace JobMagnet.Shared.Abstractions;

public interface IClock
{
    DateTimeOffset UtcNow { get; }
}

public class Clock : IClock
{
    public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
}