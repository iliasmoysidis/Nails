using Application.Commands.Appointments;

namespace Application.Abstractions.Policies.Appointments;

public interface IRescheduleAppointmentPolicy
{
    Task EnsureCanRescheduleAsync(RescheduleAppointmentCommand command, CancellationToken ct);
}