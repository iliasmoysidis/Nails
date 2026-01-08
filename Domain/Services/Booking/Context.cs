using Domain.Entities;

namespace Domain.Services.Booking;

public sealed class Context
{
    public StoreCatalog StoreCatalog { get; }
    public StoreCalendar StoreCalendar { get; }
    public StaffCalendar StaffCalendar { get; }
    public Staff Staff { get; }
    public IReadOnlyCollection<Appointment> Appointments { get; }

    public Context(
        StoreCatalog storeCatalog,
        StoreCalendar storeCalendar,
        StaffCalendar staffCalendar,
        Staff staff,
        IReadOnlyCollection<Appointment> appointments
    )
    {
        StoreCatalog = storeCatalog;
        StoreCalendar = storeCalendar;
        StaffCalendar = staffCalendar;
        Staff = staff;
        Appointments = appointments;
    }
}