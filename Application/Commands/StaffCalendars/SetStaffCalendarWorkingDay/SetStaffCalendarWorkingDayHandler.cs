using Domain.ValueObjects.Calendar;
using MediatR;

namespace Application.Commands.StaffCalendars;

public sealed class SetStaffCalendarWorkingDayHandler
    : IRequestHandler<SetStaffCalendarWorkingDayCommand>
{
    private readonly SetStaffCalendarWorkingDayContext _ctx;

    public SetStaffCalendarWorkingDayHandler(
        SetStaffCalendarWorkingDayContext ctx)
    {
        _ctx = ctx;
    }

    public Task Handle(
        SetStaffCalendarWorkingDayCommand command,
        CancellationToken ct)
    {
        var ranges = command.TimeRanges.Select(r => new TimeRange(r.Start, r.End));
        var workingDay = WorkingDay.WithRanges(command.Day, ranges);

        _ctx.StaffCalendar.SetWorkingDay(workingDay);

        return Task.CompletedTask;
    }
}