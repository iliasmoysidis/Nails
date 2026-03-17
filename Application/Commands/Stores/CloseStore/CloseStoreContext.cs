using Domain.Entities;

namespace Application.Commands.Stores;

public sealed class CloseStoreContext
{
    public Store Store { get; set; } = default!;
    public Staff Staff { get; set; } = default!;
    public StoreCatalog? Catalog { get; set; }
    public Domain.Entities.Assignments? Assignments { get; set; }
    public StoreCalendar? StoreCalendar { get; set; }
    public IReadOnlyCollection<StaffCalendar> StaffCalendars { get; set; } = [];
    public IReadOnlyCollection<Appointment> UpcomingAppointments { get; set; } = [];
}