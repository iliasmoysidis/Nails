using Domain.Common.Exceptions;

namespace Domain.Common.ValueObjects;

public sealed record UtcDateTime : IComparable<UtcDateTime>
{
    public DateTime Value { get; }

    private UtcDateTime(DateTime value)
    {
        if (value.Kind != DateTimeKind.Utc)
            throw new ValidationException("UtcDateTime must be in UTC.");

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

    public UtcDateTime AddMinutes(int minutes)
        => FromUtc(Value.AddMinutes(minutes));

    public UtcDateTime AddHours(int hours)
        => FromUtc(Value.AddHours(hours));

    public DateOnly Date => DateOnly.FromDateTime(Value);

    public DayOfWeek DayOfWeek => Value.DayOfWeek;

    public TimeSpan TimeOfDay => Value.TimeOfDay;

    public int Hour => Value.Hour;
    public int Minute => Value.Minute;
    public int Second => Value.Second;

    public int CompareTo(UtcDateTime? other)
        => other is null ? 1 : Value.CompareTo(other.Value);

    public static TimeSpan operator -(UtcDateTime a, UtcDateTime b)
        => a.Value - b.Value;

    public static bool operator >(UtcDateTime a, UtcDateTime b) => a.Value > b.Value;
    public static bool operator <(UtcDateTime a, UtcDateTime b) => a.Value < b.Value;
    public static bool operator >=(UtcDateTime a, UtcDateTime b) => a.Value >= b.Value;
    public static bool operator <=(UtcDateTime a, UtcDateTime b) => a.Value <= b.Value;

    public bool IsBefore(UtcDateTime other)
        => Value < other.Value;

    public bool IsAfter(UtcDateTime other)
        => Value > other.Value;

    public override string ToString()
        => Value.ToString("O");
}
