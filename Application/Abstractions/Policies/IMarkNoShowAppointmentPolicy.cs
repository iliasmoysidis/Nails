using Application.Commands.Appointments;

namespace Application.Abstractions.Policies;

public interface IMarkNoShowAppointmentPolicy
{
    Task EnsureCanMarkNoShowAsync(MarkNoShowAppointmentCommand command, CancellationToken ct);
}