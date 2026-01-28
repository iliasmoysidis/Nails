using Application.Abstractions.Policies;
using Application.Abstractions.Repositories;
using Application.Commands.Appointments;
using Application.Contexts;
using Application.Exceptions;

namespace Application.Authorization;

public sealed class ConfirmAppointmentPolicy : IConfirmAppointmentPolicy
{
    private readonly IRequestContext _context;
    private readonly IAppointmentRepository _appointmentRepo;
    private readonly IStaffRepository _staffRepo;

    public ConfirmAppointmentPolicy(IAppointmentRepository appointmentRepo, IStaffRepository staffRepo, IRequestContext context)
    {
        _context = context;
        _appointmentRepo = appointmentRepo;
        _staffRepo = staffRepo;
    }

    public async Task EnsureCanConfirmAsync(ConfirmAppointmentCommand command, CancellationToken ct)
    {
        if (!_context.IsProfessional) throw Forbidden();

        var appointment = await _appointmentRepo.GetByIdAsync(command.AppointmentId, ct);
        if (appointment is null) throw Forbidden();


        var staff = await _staffRepo.GetByStoreId(appointment.StoreId, ct);

        if (staff is null || !staff.IsStaff(_context.ActorId)) throw Forbidden();
    }

    private static ApplicationLayerForbiddenException Forbidden()
        => new("Not allowed to confirm appointment.");
}