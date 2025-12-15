namespace Domain.ValueObjects.Time;

public readonly struct UtcDateTime : IComparable<UtcDateTime>
{
    public DateTime Value { get; }

    public UtcDateTime(DateTime value)
    {
        if (value.Kind != DateTimeKind.Utc)
        {
            throw new InvalidOperationException("UtcDateTime must be in UTC.");
        }

        Value = value;
    }

    public static UtcDateTime From(DateTime dateTime)
        => new(dateTime.Kind == DateTimeKind.Utc
            ? dateTime
            : dateTime.ToUniversalTime());

    public static UtcDateTime Now()
        => new(DateTime.UtcNow);

    public UtcDateTime AddMinutes(double minutes)
        => new(Value.AddMinutes(minutes));

    public UtcDateTime AddHours(double hours)
        => new(Value.AddHours(hours));

    public UtcDateTime Add(TimeSpan duration)
        => new(Value.Add(duration));

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

    public override string ToString()
        => Value.ToString("O");
}