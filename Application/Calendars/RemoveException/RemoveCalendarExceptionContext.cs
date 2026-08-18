using Domain.Rosters;
using Domain.Calendars.Services;

namespace Application.Calendars.RemoveException;

public sealed class RemoveCalendarExceptionContext
{
    public Staff Staff { get; set; } = default!;
    public StoreAvailability StoreAvailability { get; set; } = default!;
}
