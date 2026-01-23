using Domain.Entities;

namespace Application.Policies.Interfaces;

public interface IAppointmentVisibilityPolicy
{
    Task EnsureCanViewAsync(Appointment appointment, int agentId, CancellationToken ct);
}