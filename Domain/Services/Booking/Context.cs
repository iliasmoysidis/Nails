using Domain.Entities;

namespace Domain.Services.Booking;

public sealed class BookingContext
{
    public StoreCatalog StoreCatalog { get; }
    public StoreCalendar StoreCalendar { get; }
    public StaffCalendar StaffCalendar { get; }
    public Staff Staff { get; }
    public IReadOnlyCollection<Appointment> Appointments { get; }

    public BookingContext(
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