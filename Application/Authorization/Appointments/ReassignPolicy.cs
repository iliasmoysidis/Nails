using Application.Abstractions.Policies.Appointments;
using Application.Abstractions.Repositories;
using Application.Commands.Appointments;
using Application.Contexts;
using Application.Exceptions;

namespace Application.Authorization.Appointments;

public sealed class ReassignPolicy : IReassignPolicy
{
    private readonly IRequestContext _context;
    private readonly IAppointmentRepository _appointmentRepo;
    private readonly IStaffRepository _staffRepo;

    public ReassignPolicy(
        IRequestContext context,
        IAppointmentRepository appointmentRepo,
        IStaffRepository staffRepo
    )
    {
        _context = context;
        _appointmentRepo = appointmentRepo;
        _staffRepo = staffRepo;
    }

    public async Task EnsureCanReassignAsync(ReassignCommand command, CancellationToken ct)
    {
        if (!_context.IsProfessional) throw Forbidden();

        var appointment = await _appointmentRepo.GetByIdAsync(command.AppointmentId, ct)
            ?? throw new ApplicationLayerNotFoundException("Appointment not found.");

        var staff = await _staffRepo.GetByStoreId(appointment.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Staff not found.");

        if (!staff.IsOwner(_context.ActorId)) throw Forbidden();

        if (!staff.IsEmployee(command.ProfessionalId)) throw Forbidden();

        return;
    }

    private static ApplicationLayerForbiddenException Forbidden()
        => new("Not allowed to reassign appointment.");
}