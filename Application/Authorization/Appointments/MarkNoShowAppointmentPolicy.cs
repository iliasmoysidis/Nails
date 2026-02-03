using Application.Abstractions.Policies.Appointments;
using Application.Abstractions.Repositories;
using Application.Commands.Appointments;
using Application.Contexts;
using Application.Exceptions;

namespace Application.Authorization.Appointments;

public sealed class MarkNoShowAppointmentPolicy : IMarkNoShowAppointmentPolicy
{
    private readonly IRequestContext _context;
    private readonly IAppointmentRepository _appointmentRepo;
    private readonly IStaffRepository _staffRepo;

    public MarkNoShowAppointmentPolicy(IAppointmentRepository appointmentRepo, IStaffRepository staffRepo, IRequestContext context)
    {
        _context = context;
        _appointmentRepo = appointmentRepo;
        _staffRepo = staffRepo;
    }

    public async Task EnsureCanMarkNoShowAsync(MarkNoShowAppointmentCommand command, CancellationToken ct)
    {
        if (!_context.IsProfessional) throw Forbidden();

        var appointment = await _appointmentRepo.GetByIdAsync(command.AppointmentId, ct)
            ?? throw Forbidden();

        var staff = await _staffRepo.GetByStoreId(appointment.StoreId, ct)
            ?? throw Forbidden();

        if (!staff.IsStaff(_context.ActorId)) throw Forbidden();
    }

    private static ApplicationLayerForbiddenException Forbidden()
        => new("Not allowed to mark appointment as no-show.");
}