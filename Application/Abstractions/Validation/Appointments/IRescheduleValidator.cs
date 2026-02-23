using Application.Commands.Appointments;

namespace Application.Abstractions.Validation.Appointments;

public interface IRescheduleValidator
{
    Task EnsureAvailableAsync(RescheduleAppointmentCommand command, CancellationToken ct);
}