using Domain.Exceptions;

namespace Domain.ValueObjects.Calendar;

public sealed class WorkingDay
{
    public DayOfWeek Day { get; }
    public IReadOnlyCollection<TimeRange> TimeRanges { get; }

    public bool IsDayOff => !TimeRanges.Any();

    private WorkingDay(DayOfWeek day, IReadOnlyCollection<TimeRange> ranges)
    {
        Day = day;
        TimeRanges = ranges;
    }

    public static WorkingDay DayOff(DayOfWeek day)
        => new(day, Array.Empty<TimeRange>());

    public static WorkingDay WithRanges(DayOfWeek day, IEnumerable<TimeRange> ranges)
    {
        var list = ranges
            .OrderBy(r => r.Start)
            .ToList();

        if (!list.Any())
        {
            throw new DomainException("Working day must contain at least one time range.");
        }

        for (int i = 0; i < list.Count - 1; i++)
        {
            if (list[i].Overlaps(list[i + 1]))
                throw new DomainException("Working time ranges cannot overlap.");
        }

        return new WorkingDay(day, list);
    }

    public bool IsWorkingAt(TimeSpan time)
        => TimeRanges.Any(r => r.Contains(time));
}