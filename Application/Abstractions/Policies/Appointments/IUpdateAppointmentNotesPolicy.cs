using Application.Commands.Appointments;

namespace Application.Abstractions.Policies.Appointments;

public interface IUpdateAppointmentNotesPolicy
{
    Task EnsureCanUpdateAsync(UpdateAppointmentNotesCommand command, CancellationToken ct);
}