using Domain.Entities;
using Domain.Interfaces;

namespace Domain.Services;

public sealed class StoreClosureService : IStoreClosureService
{
    public void CloseStore(
        Store store,
        Staff staff,
        StoreCatalog? catalog,
        ProfessionalOfferings? assignments,
        StoreCalendar? storeCalendar,
        IReadOnlyCollection<StaffCalendar> staffCalendars,
        IReadOnlyCollection<Appointment> upcomingAppointments,
        IClock clock
    )
    {
        foreach (var appointment in upcomingAppointments)
        {
            appointment.Cancel(clock, "Store has been closed.");
        }

        staff.Clear(clock);
        catalog?.Clear(clock);
        assignments?.Clear();
        storeCalendar?.Clear();

        foreach (var staffCalendar in staffCalendars)
        {
            staffCalendar.Clear();
        }

        store.SoftDelete(clock);
    }

}