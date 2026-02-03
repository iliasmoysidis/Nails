using Application.Commands.Appointments;

namespace Application.Abstractions.Policies.Appointments;

public interface IMarkNoShowAppointmentPolicy
{
    Task EnsureCanMarkNoShowAsync(MarkNoShowAppointmentCommand command, CancellationToken ct);
}