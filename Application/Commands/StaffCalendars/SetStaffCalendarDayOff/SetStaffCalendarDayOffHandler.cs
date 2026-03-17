using MediatR;

namespace Application.Commands.StaffCalendars;

public sealed class SetStaffCalendarDayOffHandler
    : IRequestHandler<SetStaffCalendarDayOffCommand>
{
    private readonly SetStaffCalendarDayOffContext _ctx;

    public SetStaffCalendarDayOffHandler(
        SetStaffCalendarDayOffContext ctx)
    {
        _ctx = ctx;
    }

    public Task Handle(
        SetStaffCalendarDayOffCommand command,
        CancellationToken ct)
    {
        _ctx.StaffCalendar.SetDayOff(command.Day);

        return Task.CompletedTask;
    }
}