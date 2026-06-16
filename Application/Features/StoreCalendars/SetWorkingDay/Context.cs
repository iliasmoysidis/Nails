using Domain.Entities;
using Domain.Services;

namespace Application.Features.StoreCalendars.SetWorkingDay;

public sealed class Context
{
    public Staff Staff { get; set; } = default!;
    public StoreAvailability StoreAvailability { get; set; } = default!;
}
