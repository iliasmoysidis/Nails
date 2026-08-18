using Domain.Common.ValueObjects.Calendars;
using MediatR;

namespace Application.Schedules.AddVacation;

public sealed class AddVacationHandler
    : IRequestHandler<AddVacationCommand>
{
    private readonly AddVacationContext _ctx;

    public AddVacationHandler(
        AddVacationContext ctx)
    {
        _ctx = ctx;
    }

    public Task Handle(
        AddVacationCommand command,
        CancellationToken ct)
    {
        var vacation = CalendarException.DayOff(command.Date);

        _ctx.ProfessionalAvailability.SetException(vacation);

        return Task.CompletedTask;
    }
}
