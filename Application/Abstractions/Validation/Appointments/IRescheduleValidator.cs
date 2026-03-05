using Domain.Entities;
using Domain.ValueObjects.Time;

namespace Application.Abstractions.Validation.Appointments;

public interface IRescheduleValidator
{
    void EnsureAvailable(
        StoreCalendar storeCalendar,
        StaffCalendar staffCalendar,
        Appointment appointment,
        IReadOnlyCollection<Appointment> appointments,
        UtcDateTime newStartAt
    );
}