using Domain.ValueObjects.Calendar;
using MediatR;

namespace Application.Commands.StoreCalendars;

public sealed class SetStoreCalendarWorkingDayHandler
    : IRequestHandler<SetStoreCalendarWorkingDayCommand>
{
    private readonly SetStoreCalendarWorkingDayContext _ctx;

    public SetStoreCalendarWorkingDayHandler(
        SetStoreCalendarWorkingDayContext ctx)
    {
        _ctx = ctx;
    }

    public Task Handle(
        SetStoreCalendarWorkingDayCommand command,
        CancellationToken ct)
    {
        var ranges = command.TimeRanges
            .Select(r => new TimeRange(r.Start, r.End))
            .ToList();

        var workingDay = WorkingDay.WithRanges(command.Day, ranges);

        _ctx.StoreCalendar.SetWorkingDay(workingDay);

        return Task.CompletedTask;
    }
}