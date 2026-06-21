using Domain.Roster;
using Domain.Calendar.Services;

namespace Application.Calendar.AddHoliday;

public sealed class Context
{
    public Staff Staff { get; set; } = default!;
    public StoreAvailability StoreAvailability {get; set;} = null!;
}
