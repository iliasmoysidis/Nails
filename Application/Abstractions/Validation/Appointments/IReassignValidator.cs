using Application.Commands.Appointments;

namespace Application.Abstractions.Validation.Appointments;

public interface IReassignValidator
{
    Task EnsureAvailableAsync(ReassignAppointmentCommand command, CancellationToken ct);
}