using Domain.Catalog;
using Domain.Assignments;
using Domain.Calendar;
using Domain.Appointments.ValueObjects;
using Domain.Professionals;
using Domain.Stores;
using Domain.Common;
using Domain.Common.Exceptions;
using Domain.Common.ValueObjects;
using Domain.Common.ValueObjects.Calendar;
using Domain.Schedule.Entities;

namespace Domain.Appointments.Services;

public class AppointmentBooking
{
    private readonly Professional _professional;
    private readonly Store _store;
    private readonly StoreCalendar _storeCalendar;
    private readonly StaffCalendar _staffCalendar;
    private readonly StoreCatalog _storeCatalog;
    private readonly AssignmentRegistry _assignments;

    private readonly List<Appointment> _appointments = [];

    public AppointmentBooking(
        Professional professional,
        Store store,
        StoreCalendar storeCalendar,
        StaffCalendar staffCalendar,
        StoreCatalog storeCatalog,
        AssignmentRegistry assignments,
        IReadOnlyCollection<Appointment> appointments
    )
    {
        ValidateComposition(
            professional,
            store,
            storeCalendar,
            staffCalendar,
            storeCatalog,
            assignments
        );

        _professional = professional;
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
        _professional.EnsureActive();
        EnsureProfessionalOffersService(offeringId);

        var offering = _storeCatalog.GetOffering(offeringId);
        var endAt = startAt.AddMinutes(offering.Duration.Minutes);

        EnsureAvailability(startAt, endAt);

        var appointment = Appointment.Create(
            userId: userId,
            professionalId: _professional.Id,
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
        if (!_assignments.IsAssigned(_professional.Id, offeringId))
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
        Professional professional,
        Store store,
        StoreCalendar storeCalendar,
        StaffCalendar staffCalendar,
        StoreCatalog storeCatalog,
        AssignmentRegistry assignments
    )
    {
        if (storeCalendar.StoreId != store.Id)
            throw new InvariantException("Store calendar does not belong to this store.");

        if (staffCalendar.StoreId != store.Id)
            throw new InvariantException("Staff calendar does not belong to this store.");

        if (staffCalendar.ProfessionalId != professional.Id)
            throw new InvariantException("Professional does not belong to the store.");

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

            if (appointment.ProfessionalId != _professional.Id)
                throw new InvariantException("Appointment does not belong to this professional.");

            _appointments.Add(appointment);
        }
    }
}
