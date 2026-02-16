using Application.Abstractions.Policies.Appointments;
using Application.Abstractions.Repositories;
using Application.Commands.Appointments;
using Application.Contexts;
using Application.Exceptions;

namespace Application.Authorization.Appointments;

public sealed class UpdateAppointmentNotesPolicy : IUpdateAppointmentNotesPolicy
{
    private readonly IRequestContext _context;
    private readonly IAppointmentRepository _appointmentRepo;
    private readonly IStaffRepository _staffRepo;

    public UpdateAppointmentNotesPolicy(
        IRequestContext context,
        IAppointmentRepository appointmentRepo,
        IStaffRepository staffRepo
    )
    {
        _context = context;
        _appointmentRepo = appointmentRepo;
        _staffRepo = staffRepo;
    }

    public async Task EnsureCanUpdateAsync(UpdateAppointmentNotesCommand command, CancellationToken ct)
    {
        var appointment = await _appointmentRepo.GetByIdAsync(command.AppointmentId, ct)
            ?? throw new ApplicationLayerNotFoundException("Appointment not found.");

        if (_context.IsUser)
        {
            if (_context.ActorId != appointment.UserId)
                throw Forbidden();

            return;
        }

        if (_context.IsProfessional)
        {
            var staff = await _staffRepo.GetByStoreId(appointment.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Staff not found.");

            if (!staff.IsStaff(_context.ActorId))
                throw Forbidden();

            return;
        }

        throw Forbidden();
    }

    private static ApplicationLayerForbiddenException Forbidden()
        => new("Not allowed to update appointment notes.");
}