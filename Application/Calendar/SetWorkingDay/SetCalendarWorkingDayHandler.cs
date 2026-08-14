using Domain.Common.ValueObjects.Calendar;
using MediatR;

namespace Application.Calendar.SetWorkingDay;

public sealed class SetCalendarWorkingDayHandler
    : IRequestHandler<SetCalendarWorkingDayCommand>
{
    private readonly SetCalendarWorkingDayContext _ctx;

    public SetCalendarWorkingDayHandler(SetCalendarWorkingDayContext ctx)
    {
        _ctx = ctx;
    }

    public Task Handle(
        SetCalendarWorkingDayCommand command,
        CancellationToken ct)
    {
        var ranges = command.TimeRanges
            .Select(r => new TimeRange(r.Start, r.End))
            .ToList();

        _ctx.StoreAvailability.SetOpeningHours(command.Day, ranges);

        return Task.CompletedTask;
    }
}
