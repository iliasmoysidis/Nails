using Domain.Rosters;
using Domain.Calendars.Services;

namespace Application.Calendars.AddSpecialHours;

public sealed class AddSpecialHoursContext
{
    public Staff Staff { get; set; } = default!;
    public StoreAvailability StoreAvailability { get; set; } = default!;
}
