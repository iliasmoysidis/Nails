using Application.Commands.Appointments;

namespace Application.Abstractions.Policies;

public interface IRescheduleAppointmentPolicy
{
    Task EnsureCanRescheduleAsync(RescheduleAppointmentCommand command, CancellationToken ct);
}