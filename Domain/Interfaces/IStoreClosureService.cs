using Domain.Entities;

namespace Domain.Interfaces;

public interface IStoreClosureService
{
    void CloseStore(
        Store store,
        Staff staff,
        StoreCatalog catalog,
        ProfessionalOfferings assignments,
        StoreCalendar? storeCalendar,
        IReadOnlyCollection<StaffCalendar> staffCalendars,
        IReadOnlyCollection<Appointment> upcomingAppointments,
        IClock clock
    );
}