using Domain.Roster;
using Domain.Calendar.Services;

namespace Application.Features.StoreCalendars.AddHoliday;

public sealed class Context
{
    public Staff Staff { get; set; } = default!;
    public StoreAvailability StoreAvailability {get; set;} = null!;
}
