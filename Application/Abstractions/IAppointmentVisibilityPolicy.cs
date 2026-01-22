using Domain.Entities;

namespace Application.Abstractions;

public interface IAppointmentVisibilityPolicy
{
    Task EnsureCanViewAsync(Appointment appointment, int agentId, CancellationToken ct);
}