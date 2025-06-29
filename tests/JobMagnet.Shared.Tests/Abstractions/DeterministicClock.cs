using JobMagnet.Shared.Abstractions;

namespace JobMagnet.Shared.Tests.Abstractions;

public class DeterministicClock : IClock
{
    public DeterministicClock(DateTimeOffset? startTime = null)
    {
        startTime ??= new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero);
        UtcNow = startTime.Value;
    }

    public DateTimeOffset UtcNow { get; private set; }

    public void Advance(TimeSpan duration) => UtcNow += duration;
    public void AdvanceSeconds(int seconds) => UtcNow = UtcNow.AddSeconds(seconds);
    public void Set(DateTimeOffset newTime) => UtcNow = newTime;
}