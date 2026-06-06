using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;
using Domain.ValueObjects.Appointments;
using Domain.ValueObjects.Calendar;
using Domain.ValueObjects.Time;

namespace Domain.Services;

public class AppointmentBooking
{
    private readonly Store _store;
    private readonly StoreCalendar _storeCalendar;
    private readonly StaffCalendar _staffCalendar;
    private readonly StoreCatalog _storeCatalog;
    private readonly Assignments _assignments;

    private readonly List<Appointment> _appointments = [];
    private int ProfessionalId => _staffCalendar.ProfessionalId;

    public AppointmentBooking(
        Store store,
        StoreCalendar storeCalendar,
        StaffCalendar staffCalendar,
        StoreCatalog storeCatalog,
        Assignments assignments,
        IReadOnlyCollection<Appointment> appointments
    )
    {
        ValidateComposition(
            store,
            storeCalendar,
            staffCalendar,
            storeCatalog,
            assignments
        );

        _store = store;
        _storeCalendar = storeCalendar;
        _staffCalendar = staffCalendar;
        _storeCatalog = storeCatalog;
        _assignments = assignments;

        LoadAppointments(appointments);
    }

    public Appointment Book(
        int userId,
        int offeringId,
        UtcDateTime startAt,
        Notes notes,
        IClock clock
    )
    {
        _store.EnsureOpen();
        EnsureProfessionalOffersService(offeringId);

        var offering = _storeCatalog.GetOffering(offeringId);
        var endAt = startAt.Add(offering.Duration.Value);

        EnsureAvailability(startAt, endAt);

        var appointment = Appointment.Create(
            userId: userId,
            professionalId: ProfessionalId,
            offeringId: offeringId,
            storeId: _store.Id,
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
        if (!_assignments.IsAssigned(ProfessionalId, offeringId))
            throw new InvariantException("Professional does not provide this service.");
    }

    private void EnsureAvailability(UtcDateTime startAt, UtcDateTime endAt)
    {
        var range = new TimeRange(startAt.TimeOfDay, endAt.TimeOfDay);

        if (!_storeCalendar.IsWithinStoreHours(startAt.Date, range))
            throw new InvariantException("Store is closed during the selected time.");

        if (!_staffCalendar.IsAvailable(startAt, endAt))
            throw new InvariantException("Professional is not available during the selected time.");

        foreach (var appointment in _appointments)
        {
            if (appointment.ConflictsWith(startAt, endAt))
                throw new InvariantException("Professional already has an appointment during this time.");
        }
    }

    private void ValidateComposition(
        Store store,
        StoreCalendar storeCalendar,
        StaffCalendar staffCalendar,
        StoreCatalog storeCatalog,
        Assignments assignments
    )
    {
        if (storeCalendar.StoreId != store.Id)
            throw new InvariantException("Store calendar does not belong to this store.");

        if (staffCalendar.StoreId != store.Id)
            throw new InvariantException("Staff calendar does not belong to this store.");

        if (storeCatalog.StoreId != store.Id)
            throw new InvariantException("Store catalog does not belong to this store.");

        if (assignments.StoreId != store.Id)
            throw new InvariantException("Assignments do not belong to this store.");

    }

    private void LoadAppointments(IReadOnlyCollection<Appointment> appointments)
    {
        foreach (var appointment in appointments)
        {
            if (appointment.StoreId != _store.Id)
                throw new InvariantException("Appointment does not belong to this store.");

            if (appointment.ProfessionalId != ProfessionalId)
                throw new InvariantException("Appointment does not belong to this professional.");
        }
    }
}
