using Domain.Entities;
using Domain.Exceptions;
using Domain.ValueObjects.Calendar;
using Domain.ValueObjects.Time;

namespace Domain.Services;

public sealed class AppointmentAvailabilityService
{
    public void EnsureAppointmentAvailable(
        StoreCalendar storeCalendar,
        StaffCalendar staffCalendar,
        IReadOnlyCollection<Appointment> appointments,
        UtcDateTime startAt,
        UtcDateTime endAt,
        int? ignoreAppointmentId
    )
    {
        var range = new TimeRange(startAt.TimeOfDay, endAt.TimeOfDay);

        if (!storeCalendar.IsWithinStoreHours(startAt.Date, range))
            throw new InvariantException("Store is closed during the selected time.");

        if (!staffCalendar.IsAvailable(startAt, endAt))
            throw new InvariantException("Professional is not available during the selected time");

        var conflict = appointments.Any(a =>
            !a.IsDeleted &&
            a.Id != ignoreAppointmentId &&
            a.ConflictsWith(startAt, endAt)
        );

        if (conflict)
            throw new InvariantException("Professional already has an appointment during this time.");
    }
}