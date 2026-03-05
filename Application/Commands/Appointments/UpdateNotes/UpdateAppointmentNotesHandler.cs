using Application.Abstractions.Policies.Appointments;
using Application.Abstractions.Repositories;
using Application.Abstractions.UnitOfWork;
using Application.Contexts;
using Application.Exceptions;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Commands.Appointments;

public sealed class UpdateAppointmentNotesHandler
{
    private readonly IRequestContext _context;
    private readonly IUpdateNotesPolicy _policy;
    private readonly IAppointmentRepository _appointmentRepo;
    private readonly IStaffRepository _staffRepo;
    private readonly IClock _clock;
    private readonly IUnitOfWork _uow;

    public UpdateAppointmentNotesHandler(
        IRequestContext context,
        IUpdateNotesPolicy policy,
        IAppointmentRepository appointmentRepo,
        IStaffRepository staffRepo,
        IClock clock,
        IUnitOfWork uow
    )
    {
        _context = context;
        _policy = policy;
        _appointmentRepo = appointmentRepo;
        _staffRepo = staffRepo;
        _clock = clock;
        _uow = uow;
    }

    public async Task Handle(UpdateAppointmentNotesCommand command, CancellationToken ct)
    {
        var appointment = await _appointmentRepo.GetByIdAsync(command.AppointmentId, ct)
            ?? throw new ApplicationLayerNotFoundException("Appointment not found.");

        Staff? staff = null;

        if (_context.IsProfessional)
        {
            staff = await _staffRepo.GetByStoreId(appointment.StoreId, ct)
                ?? throw new ApplicationLayerNotFoundException("Staff not found.");
        }

        _policy.EnsureCanUpdate(appointment, staff);

        appointment.UpdateNotes(command.Notes, _clock);

        await _uow.SaveChangesAsync(ct);
    }
}