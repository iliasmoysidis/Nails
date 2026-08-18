using Application.Appointments.Common.Repositories;
using Application.Rosters.Common.Repositories;
using Application.Common.Abstractions.Context;
using Application.Common.Exceptions;

namespace Application.Appointments.MarkNoShow;

public sealed class MarkAppointmentNoShowLoader
    : IRequestContextLoader<MarkAppointmentNoShowCommand, MarkAppointmentNoShowContext>
{
    private readonly IAppointmentRepository _appointmentRepo;
    private readonly IStaffRepository _staffRepo;

    public MarkAppointmentNoShowLoader(
        IAppointmentRepository appointmentRepo,
        IStaffRepository staffRepo
    )
    {
        _appointmentRepo = appointmentRepo;
        _staffRepo = staffRepo;
    }

    public async Task PopulateAsync(
        MarkAppointmentNoShowCommand command,
        MarkAppointmentNoShowContext ctx,
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