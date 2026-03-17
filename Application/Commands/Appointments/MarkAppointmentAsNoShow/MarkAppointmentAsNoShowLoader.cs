using Application.Abstractions.Repositories;
using Application.Exceptions;

namespace Application.Commands.Appointments;

public sealed class MarkAppointmentAsNoShowLoader
    : IRequestContextLoader<MarkAppointmentAsNoShowCommand, MarkAppointmentAsNoShowContext>
{
    private readonly IAppointmentRepository _appointmentRepo;
    private readonly IStaffRepository _staffRepo;

    public MarkAppointmentAsNoShowLoader(
        IAppointmentRepository appointmentRepo,
        IStaffRepository staffRepo
    )
    {
        _appointmentRepo = appointmentRepo;
        _staffRepo = staffRepo;
    }

    public async Task PopulateAsync(
        MarkAppointmentAsNoShowCommand command,
        MarkAppointmentAsNoShowContext ctx,
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