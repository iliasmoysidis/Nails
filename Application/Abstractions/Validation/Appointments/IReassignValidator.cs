using Domain.Entities;

namespace Application.Abstractions.Validation.Appointments;

public interface IReassignValidator
{
    void EnsureAvailable(
        Appointment appointment,
        IReadOnlyCollection<Appointment> appointments,
        StoreCalendar storeCalendar,
        StaffCalendar staffCalendar
    );
}