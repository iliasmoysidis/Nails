using Application.Commands.Appointments;

namespace Application.Abstractions.Validation.Appointments;

public interface IScheduleValidator
{
    Task EnsureAvailableAsync(CreateAppointmentCommand command, CancellationToken ct);
}