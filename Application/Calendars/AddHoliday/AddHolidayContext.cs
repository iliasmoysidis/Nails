using Domain.Rosters;
using Domain.Calendars.Services;

namespace Application.Calendars.AddHoliday;

public sealed class AddHolidayContext
{
    public Staff Staff { get; set; } = default!;
    public StoreAvailability StoreAvailability {get; set;} = null!;
}
