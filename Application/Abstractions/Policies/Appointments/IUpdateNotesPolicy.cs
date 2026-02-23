using Application.Commands.Appointments;

namespace Application.Abstractions.Policies.Appointments;

public interface IUpdateNotesPolicy
{
    Task EnsureCanUpdateAsync(UpdateAppointmentNotesCommand command, CancellationToken ct);
}