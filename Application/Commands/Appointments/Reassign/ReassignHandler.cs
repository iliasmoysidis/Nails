using Application.Abstractions.Policies.Appointments;
using Application.Abstractions.Repositories;
using Application.Abstractions.UnitOfWork;
using Application.Abstractions.Validation.Appointments;
using Application.Exceptions;
using Domain.Interfaces;

namespace Application.Commands.Appointments;

public sealed class ReassignHandler
{
    private readonly IReassignValidator _validator;
    private readonly IReassignPolicy _policy;
    private readonly IAppointmentRepository _appointmentRepo;
    private readonly IStaffRepository _staffRepo;
    private readonly IStoreCalendarRepository _storeCalendarRepo;
    private readonly IStaffCalendarRepository _staffCalendarRepo;
    private readonly IClock _clock;
    private readonly IUnitOfWork _uow;

    public ReassignHandler(
        IReassignValidator validator,
        IReassignPolicy policy,
        IAppointmentRepository appointmentRepo,
        IStaffRepository staffRepo,
        IStoreCalendarRepository storeCalendarRepo,
        IStaffCalendarRepository staffCalendarRepo,
        IClock clock,
        IUnitOfWork uow
    )
    {
        _validator = validator;
        _policy = policy;
        _appointmentRepo = appointmentRepo;
        _staffRepo = staffRepo;
        _storeCalendarRepo = storeCalendarRepo;
        _staffCalendarRepo = staffCalendarRepo;
        _clock = clock;
        _uow = uow;
    }

    public async Task Handle(ReassignCommand command, CancellationToken ct)
    {
        var appointment = await _appointmentRepo.GetByIdAsync(command.AppointmentId, ct)
            ?? throw new ApplicationLayerNotFoundException("Appointment not found.");

        var staff = await _staffRepo.GetByStoreId(appointment.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Staff not found.");

        var storeCalendar = await _storeCalendarRepo.GetByStoreIdAsync(appointment.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Store calendar not found.");

        var staffCalendar = await _staffCalendarRepo.GetAsync(appointment.StoreId, command.ProfessionalId, ct)
            ?? throw new ApplicationLayerValidationException("Professional calendar not found.");

        var appointments = await _appointmentRepo.GetByProfessionalIdAsync(command.ProfessionalId, ct);

        _validator.EnsureAvailable(
            appointment,
            appointments,
            storeCalendar,
            staffCalendar
        );

        _policy.EnsureCanReassign(staff, command.ProfessionalId);

        appointment.Reassign(command.ProfessionalId, _clock);

        await _uow.SaveChangesAsync(ct);
    }
}