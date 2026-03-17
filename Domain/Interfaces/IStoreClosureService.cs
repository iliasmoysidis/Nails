using Domain.Entities;

namespace Domain.Interfaces;

public interface IStoreClosureService
{
    void CloseStore(
        Staff staff,
        StoreCatalog? catalog,
        Assignments? assignments,
        StoreCalendar? storeCalendar,
        IReadOnlyCollection<StaffCalendar> staffCalendars,
        IReadOnlyCollection<Appointment> upcomingAppointments,
        IClock clock
    );
}