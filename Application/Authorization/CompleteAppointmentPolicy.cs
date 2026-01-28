using Application.Abstractions.Policies;
using Application.Abstractions.Repositories;
using Application.Commands.Appointments;
using Application.Contexts;
using Application.Exceptions;

namespace Application.Authorization;

public sealed class CompleteAppointmentPolicy : ICompleteAppointmentPolicy
{
    private readonly IRequestContext _context;
    private readonly IAppointmentRepository _appointmentRepo;
    private readonly IStaffRepository _staffRepo;

    public CompleteAppointmentPolicy(
        IRequestContext context,
        IAppointmentRepository appointmentRepo,
        IStaffRepository staffRepo
    )
    {
        _context = context;
        _appointmentRepo = appointmentRepo;
        _staffRepo = staffRepo;
    }

    public async Task EnsureCanCompleteAsync(CompleteAppointmentCommand command, CancellationToken ct)
    {
        if (!_context.IsProfessional)
            throw Forbidden();

        var appointment = await _appointmentRepo.GetByIdAsync(command.AppointmentId, ct)
            ?? throw Forbidden();

        var staff = await _staffRepo.GetByStoreId(appointment.StoreId, ct)
            ?? throw Forbidden();

        if (!staff.IsStaff(_context.ActorId))
            throw Forbidden();
    }

    private static ApplicationLayerForbiddenException Forbidden()
        => new("Not allowed to complete appointment.");
}