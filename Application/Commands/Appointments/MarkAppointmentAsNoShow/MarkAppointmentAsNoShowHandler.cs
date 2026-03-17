using Application.Abstractions.Repositories;
using Application.Guards;
using Application.Exceptions;
using Domain.Interfaces;

namespace Application.Commands.Appointments;

public sealed class MarkAppointmentAsNoShowHandler
{
    private readonly AuthorizationGuard _auth;
    private readonly IAppointmentRepository _appointmentRepo;
    private readonly IStaffRepository _staffRepo;
    private readonly IClock _clock;

    public MarkAppointmentAsNoShowHandler(
        AuthorizationGuard auth,
        IAppointmentRepository appointmentRepo,
        IStaffRepository staffRepo,
        IClock clock
    )
    {
        _auth = auth;
        _appointmentRepo = appointmentRepo;
        _staffRepo = staffRepo;
        _clock = clock;
    }

    public async Task Handle(MarkAppointmentAsNoShowCommand command, CancellationToken ct)
    {
        var appointment = await _appointmentRepo.GetByIdAsync(command.AppointmentId, ct)
            ?? throw new ApplicationLayerNotFoundException("Appointment not found.");

        var staff = await _staffRepo.GetByStoreIdAsync(appointment.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Staff not found");

        _auth.EnsureStaffMember(staff);

        appointment.MarkAsNoShow(_clock);
    }
}