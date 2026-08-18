using Domain.Rosters;
using Domain.Calendars.Services;

namespace Application.Calendars.SetWorkingDay;

public sealed class SetCalendarWorkingDayContext
{
    public Staff Staff { get; set; } = default!;
    public StoreAvailability StoreAvailability { get; set; } = default!;
}
