using Domain.Common.Exceptions;

namespace Domain.Common.ValueObjects;

public sealed record Duration
{
    public const int SchedulingGranuleMinutes = 15;

    public int Minutes { get; }

    private Duration(int minutes)
    {
        Minutes = minutes;
    }

    public static Duration FromMinutes(int minutes)
    {
        if (minutes <= 0)
            throw new ValidationException("Duration must be positive.");

        if (minutes % SchedulingGranuleMinutes != 0)
            throw new ValidationException($"Duration must be in {SchedulingGranuleMinutes}-minute increments.");

        if (minutes > 8 * 60)
            throw new ValidationException("Duration cannot exceed 8 hours.");

        return new Duration(minutes);
    }

    public override string ToString()
        => $"{Minutes} minutes";
}
