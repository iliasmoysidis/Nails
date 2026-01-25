using Domain.Exceptions;

namespace Domain.ValueObjects.Calendar;

public sealed class CalendarException
{
    public DateOnly Date { get; }
    public IReadOnlyCollection<TimeRange> TimeRanges { get; }

    public bool IsDayOff => !TimeRanges.Any();

    private CalendarException(DateOnly date, IReadOnlyCollection<TimeRange> ranges)
    {
        Date = date;
        TimeRanges = ranges;
    }

    public static CalendarException DayOff(DateOnly date)
        => new(date, Array.Empty<TimeRange>());

    public static CalendarException PartialDay(DateOnly date, IEnumerable<TimeRange> ranges)
    {
        var list = ranges
            .OrderBy(r => r.Start)
            .ToList();

        for (int i = 0; i < list.Count - 1; i++)
        {
            if (list[i].Overlaps(list[i + 1]))
            {
                throw new ValidationException("Calendar exception time ranges cannot overlap.");
            }
        }

        return new CalendarException(date, list.AsReadOnly());
    }
}