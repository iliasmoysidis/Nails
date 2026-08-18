using Domain.Common.ValueObjects.Calendars;
using MediatR;

namespace Application.Schedules.SetWorkingDay;

public sealed class SetScheduleWorkingDayHandler
    : IRequestHandler<SetScheduleWorkingDayCommand>
{
    private readonly SetScheduleWorkingDayContext _ctx;

    public SetScheduleWorkingDayHandler(SetScheduleWorkingDayContext ctx)
    {
        _ctx = ctx;
    }

    public Task Handle(
        SetScheduleWorkingDayCommand command,
        CancellationToken ct)
    {
        var ranges = command.TimeRanges.Select(r => new TimeRange(r.Start, r.End));
        var workingDay = WorkingDay.WithRanges(command.Day, ranges);

        _ctx.ProfessionalAvailability.SetWorkingDay(workingDay);

        return Task.CompletedTask;
    }
}
