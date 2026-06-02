using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;
using Domain.ValueObjects.Appointments;
using Domain.ValueObjects.Calendar;
using Domain.ValueObjects.Time;

namespace Domain.Services;

public class AppointmentBooking
{
    public int StoreId { get; }
    public int ProfessionalId { get; }

    public StoreCalendar StoreCalendar { get; }
    public StaffCalendar StaffCalendar { get; }
    public StoreCatalog StoreCatalog { get; }
    public Assignments Assignments { get; }

    private readonly List<Appointment> _appointments = [];

    public AppointmentBooking(
        int storeId,
        int professionalId,
        StoreCalendar storeCalendar,
        StaffCalendar staffCalendar,
        StoreCatalog storeCatalog,
        Assignments assignments,
        IReadOnlyCollection<Appointment> appointments
    )
    {
        if (storeCalendar.StoreId != storeId)
            throw new InvariantException("Store calendar does not belong to this store.");

        if (staffCalendar.StoreId != storeId)
            throw new InvariantException("Staff calendar does not belong to this store.");

        if (staffCalendar.ProfessionalId != professionalId)
            throw new InvariantException("Staff calendar does not belong to this professional.");

        if (storeCatalog.StoreId != storeId)
            throw new InvariantException("Store catalog does not belong to this store.");

        if (assignments.StoreId != storeId)
            throw new InvariantException("Assignments do not belong to this store.");

        StoreId = storeId;
        ProfessionalId = professionalId;

        StoreCalendar = storeCalendar;
        StaffCalendar = staffCalendar;
        StoreCatalog = storeCatalog;
        Assignments = assignments;

        foreach (var appointment in appointments)
        {
            if (appointment.StoreId != storeId)
                throw new InvariantException("Appointment does not belong to this store.");

            if (appointment.ProfessionalId != professionalId)
                throw new InvariantException("Appointment does not belong to this professional.");

            _appointments.Add(appointment);
        }
    }

    public Appointment Book(
        int userId,
        int offeringId,
        UtcDateTime startAt,
        Notes notes,
        IClock clock
    )
    {
        EnsureProfessionalOffersService(offeringId);

        var offering = StoreCatalog.GetOffering(offeringId);
        var endAt = startAt.Add(offering.Duration.Value);

        EnsureAvailability(startAt, endAt);

        var appointment = Appointment.Create(
            userId: userId,
            professionalId: ProfessionalId,
            offeringId: offeringId,
            storeId: StoreId,
            startAt: startAt,
            duration: offering.Duration,
            price: offering.Price,
            notes: notes,
            clock: clock
        );

        _appointments.Add(appointment);

        return appointment;
    }

    private void EnsureProfessionalOffersService(int offeringId)
    {
        if (!Assignments.IsAssigned(ProfessionalId, offeringId))
            throw new InvariantException("Professional does not provide this service.");
    }

    private void EnsureAvailability(UtcDateTime startAt, UtcDateTime endAt)
    {
        var range = new TimeRange(startAt.TimeOfDay, endAt.TimeOfDay);

        if (!StoreCalendar.IsWithinStoreHours(startAt.Date, range))
            throw new InvariantException("Store is closed during the selected time.");

        if (!StaffCalendar.IsAvailable(startAt, endAt))
            throw new InvariantException("Professional is not available during the selected time.");

        foreach (var appointment in _appointments)
        {
            if (appointment.ConflictsWith(startAt, endAt))
                throw new InvariantException("Professional already has an appointment during this time.");
        }
    }
}
