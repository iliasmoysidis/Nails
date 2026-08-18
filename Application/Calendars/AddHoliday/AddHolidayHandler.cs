using MediatR;

namespace Application.Calendars.AddHoliday;

public sealed class AddHolidayHandler
    : IRequestHandler<AddHolidayCommand>
{
    private readonly AddHolidayContext _ctx;

    public AddHolidayHandler(AddHolidayContext ctx)
    {
        _ctx = ctx;
    }

    public Task Handle(
        AddHolidayCommand command,
        CancellationToken ct
    )
    {
        _ctx.StoreAvailability.AddHoliday(command.Date);

        return Task.CompletedTask;
    }
}
