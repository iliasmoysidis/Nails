using Domain.Exceptions;

namespace Domain.ValueObjects.Time;

public sealed record Duration
{
    public TimeSpan Value { get; }

    private Duration(TimeSpan value)
    {
        Value = value;
    }

    public static Duration FromMinutes(int minutes)
    {
        if (minutes <= 0)
            throw new ValidationException("Duration must be positive.");

        if (minutes % 15 != 0)
            throw new ValidationException("Duration must be in 15-minute increments.");

        if (minutes > 8 * 60)
            throw new ValidationException("Duration cannot exceed 8 hours.");

        return new Duration(TimeSpan.FromMinutes(minutes));
    }

    public static Duration FromTimeSpan(TimeSpan value)
    {
        if (value.TotalMinutes % 1 != 0)
            throw new ValidationException("Duration must be whole minutes");

        return FromMinutes((int)value.TotalMinutes);
    }


    public override string ToString()
        => $"{Value.TotalMinutes} minutes";
}