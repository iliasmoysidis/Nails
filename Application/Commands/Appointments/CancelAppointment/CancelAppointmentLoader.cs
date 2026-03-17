using Application.Abstractions.Repositories;
using Application.Exceptions;

namespace Application.Commands.Appointments;

public sealed class CancelAppointmentLoader
    : IRequestContextLoader<CancelAppointmentCommand, CancelAppointmentContext>
{
    private readonly IAppointmentRepository _appointmentRepo;
    private readonly IStaffRepository _staffRepo;

    public CancelAppointmentLoader(
        IAppointmentRepository appointmentRepo,
        IStaffRepository staffRepo
    )
    {
        _appointmentRepo = appointmentRepo;
        _staffRepo = staffRepo;
    }

    public async Task PopulateAsync(
        CancelAppointmentCommand command,
        CancelAppointmentContext ctx,
        CancellationToken ct
    )
    {
        ctx.Appointment = await _appointmentRepo.GetByIdAsync(command.AppointmentId, ct)
            ?? throw new ApplicationLayerNotFoundException("Appointment not found.");

        ctx.Staff = await _staffRepo.GetByStoreIdAsync(ctx.Appointment.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Staff not found.");
    }
}