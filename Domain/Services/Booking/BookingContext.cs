using Domain.Entities;
using Domain.Exceptions;

namespace Domain.Services.Booking;

public sealed class BookingContext
{
    public StoreCatalog StoreCatalog { get; }
    public StoreCalendar StoreCalendar { get; }
    public StaffCalendar StaffCalendar { get; }
    public Staff Staff { get; }
    public IReadOnlyCollection<Appointment> Appointments { get; }

    internal BookingContext(
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

        EnsureConsistency();
    }

    private void EnsureConsistency()
    {
        if (StoreCatalog.StoreId != StoreCalendar.StoreId)
            throw new DomainException("Store catalog and store calendar do not belong to the same store.");

        if (StoreCatalog.StoreId != Staff.StoreId)
            throw new DomainException("Staff does not belong to the store.");

        if (StaffCalendar.StoreId != StoreCatalog.StoreId)
            throw new DomainException("Staff calendar does not belong to the store.");

        if (!Staff.IsEmployee(StaffCalendar.ProfessionalId))
            throw new DomainException("Staff calendar professional is not part of the staff.");

        if (Appointments.Any(a => a.StoreId != StoreCatalog.StoreId || a.ProfessionalId != StaffCalendar.ProfessionalId))
            throw new DomainException("Appointments do not match booking context.");
    }
}