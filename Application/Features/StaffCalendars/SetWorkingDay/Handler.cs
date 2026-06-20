using Domain.Common.ValueObjects.Calendar;
using MediatR;

namespace Application.Features.StaffCalendars.SetWorkingDay;

public sealed class Handler
    : IRequestHandler<Command>
{
    private readonly Context _ctx;

    public Handler(Context ctx)
    {
        _ctx = ctx;
    }

    public Task Handle(
        Command command,
        CancellationToken ct)
    {
        var ranges = command.TimeRanges.Select(r => new TimeRange(r.Start, r.End));
        var workingDay = WorkingDay.WithRanges(command.Day, ranges);

        _ctx.ProfessionalAvailability.SetWorkingDay(workingDay);

        return Task.CompletedTask;
    }
}
