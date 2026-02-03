using Application.Commands.Appointments;

namespace Application.Abstractions.Policies.Appointments;

public interface ICreateAppointmentPolicy
{
    Task EnsureCanCreateAsync(CreateAppointmentCommand command, CancellationToken ct);
}