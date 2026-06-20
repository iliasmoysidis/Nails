using Domain.Schedule;
using Domain.Schedule.Entities;
using Domain.Professionals;
using Domain.Stores;
using Domain.Common.Exceptions;

namespace Domain.Roster.Services;

public sealed class EmploymentCreation
{
    private readonly Store _store;
    private readonly Professional _professional;
    private readonly Staff _staff;
    private readonly ProfessionalSchedule _professionalSchedule;

    public EmploymentCreation(
        Store store,
        Professional professional,
        Staff staff,
        ProfessionalSchedule professionalSchedule
    )
    {
        ValidateComposition(store, professional, staff, professionalSchedule);

        _store = store;
        _professional = professional;
        _staff = staff;
        _professionalSchedule = professionalSchedule;
    }

    public void Hire()
    {
        _store.EnsureOpen();
        _professional.EnsureActive();

        _staff.AddEmployee(_professional.Id);

        var calendar = new StaffCalendar(_store.Id, _professional.Id);

        _professionalSchedule.AddCalendar(calendar);
    }

    private void ValidateComposition(Store store,
        Professional professional,
        Staff staff,
        ProfessionalSchedule professionalSchedule
    )
    {
        if (staff.StoreId != store.Id)
            throw new InvariantException("Staff does not belong to the store.");

        if (professionalSchedule.ProfessionalId != professional.Id)
            throw new InvariantException("Schedule does not belong to the professional.");
    }
}
