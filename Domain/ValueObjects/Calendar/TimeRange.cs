using Domain.Exceptions;

namespace Domain.ValueObjects.Calendar;

public sealed record TimeRange
{
    public TimeSpan Start { get; }
    public TimeSpan End { get; }

    public TimeRange(TimeSpan start, TimeSpan end)
    {
        if (start >= end)
        {
            throw new DomainException("Start time must be before end time.");
        }

        Start = start;
        End = end;
    }

    public bool Overlaps(TimeRange other)
        => Start < other.End && End > other.Start;

    public bool Contains(TimeSpan time)
        => time >= Start && time < End;

    public TimeSpan Duration => End - Start;
}