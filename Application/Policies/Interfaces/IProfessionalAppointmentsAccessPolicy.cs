namespace Application.Policies.Interfaces;

public interface IProfessionalAppointmentsAccessPolicy
{
    Task EnsureCanViewAsync(int agentId, int storeId, int professionalId, CancellationToken ct);
}