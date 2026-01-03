using Domain.Interfaces;
using Domain.ValueObjects.Time;

namespace Fakes;

public sealed class FakeClock : IClock
{
    public UtcDateTime Now { get; private set; }

    public FakeClock(UtcDateTime now)
    {
        Now = now;
    }

    public void Advance(TimeSpan duration)
    {
        Now = Now.Add(duration);
    }
}