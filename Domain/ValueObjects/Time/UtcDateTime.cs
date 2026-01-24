using Domain.Exceptions;

namespace Domain.ValueObjects.Time;

public readonly struct UtcDateTime : IComparable<UtcDateTime>
{
    public DateTime Value { get; }

    private UtcDateTime(DateTime value)
    {
        if (value.Kind != DateTimeKind.Utc)
            throw new DomainException("UtcDateTime must be in UTC.");

        Value = value;
    }

    public static UtcDateTime FromUtc(DateTime dateTime)
    {
        return new UtcDateTime(dateTime);
    }

    public static UtcDateTime Now()
        => FromUtc(DateTime.UtcNow);

    public UtcDateTime Add(TimeSpan duration)
        => FromUtc(Value.Add(duration));

    public DateOnly Date => DateOnly.FromDateTime(Value);

    public DayOfWeek DayOfWeek => Value.DayOfWeek;

    public TimeSpan TimeOfDay => Value.TimeOfDay;

    public int Hour => Value.Hour;
    public int Minute => Value.Minute;
    public int Second => Value.Second;

    public int CompareTo(UtcDateTime other)
        => Value.CompareTo(other.Value);

    public static TimeSpan operator -(UtcDateTime a, UtcDateTime b)
        => a.Value - b.Value;

    public static bool operator >(UtcDateTime a, UtcDateTime b) => a.Value > b.Value;
    public static bool operator <(UtcDateTime a, UtcDateTime b) => a.Value < b.Value;
    public static bool operator >=(UtcDateTime a, UtcDateTime b) => a.Value >= b.Value;
    public static bool operator <=(UtcDateTime a, UtcDateTime b) => a.Value <= b.Value;
    public static bool operator ==(UtcDateTime a, UtcDateTime b)
        => a.Value == b.Value;
    public static bool operator !=(UtcDateTime a, UtcDateTime b)
        => a.Value != b.Value;

    public override bool Equals(object? obj)
        => obj is UtcDateTime other && Value == other.Value;

    public override int GetHashCode()
        => Value.GetHashCode();

    public override string ToString()
        => Value.ToString("O");
}