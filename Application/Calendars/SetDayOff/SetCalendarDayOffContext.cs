using Domain.Rosters;
using Domain.Calendars.Services;

namespace Application.Calendars.SetDayOff;

public sealed class SetCalendarDayOffContext
{
    public Staff Staff { get; set; } = default!;
    public StoreAvailability StoreAvailability { get; set; } = default!;
}
