using Application.Commands.Appointments;

namespace Application.Abstractions.Policies;

public interface ICreateAppointmentPolicy
{
    Task EnsureCanCreateAsync(CreateAppointmentCommand command, CancellationToken ct);
}