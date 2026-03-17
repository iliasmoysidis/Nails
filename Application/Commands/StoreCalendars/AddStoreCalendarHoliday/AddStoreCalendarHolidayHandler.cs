using Domain.ValueObjects.Calendar;
using MediatR;

namespace Application.Commands.StoreCalendars;

public sealed class AddStoreCalendarHolidayHandler
    : IRequestHandler<AddStoreCalendarHolidayCommand>
{
    private readonly AddStoreCalendarHolidayContext _ctx;

    public AddStoreCalendarHolidayHandler(
        AddStoreCalendarHolidayContext ctx)
    {
        _ctx = ctx;
    }

    public Task Handle(
        AddStoreCalendarHolidayCommand command,
        CancellationToken ct)
    {
        var holiday = CalendarException.DayOff(command.Date);

        _ctx.StoreCalendar.AddException(holiday);

        return Task.CompletedTask;
    }
}