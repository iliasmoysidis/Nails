using Application.Appointments.Common.Repositories;
using Application.Rosters.Common.Repositories;
using Application.Common.Abstractions.Context;
using Application.Common.Exceptions;

namespace Application.Appointments.Cancel;

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
        var appointment = await _appointmentRepo.GetByIdAsync(command.AppointmentId, ct)
            ?? throw new ApplicationLayerNotFoundException("Appointment not found.");

        var staff = await _staffRepo.GetByStoreIdAsync(appointment.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Staff not found.");

        ctx.Appointment = appointment;
        ctx.Staff = staff;
    }
}