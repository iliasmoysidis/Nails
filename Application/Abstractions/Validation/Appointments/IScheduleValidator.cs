using Domain.Entities;
using Domain.ValueObjects.Time;

namespace Application.Abstractions.Validation.Appointments;

public interface IScheduleValidator
{
    void EnsureAvailable(
        StoreCalendar storeCalendar,
        StaffCalendar staffCalendar,
        IReadOnlyCollection<Appointment> appointments,
        UtcDateTime startAt,
        UtcDateTime endAt
        );
}