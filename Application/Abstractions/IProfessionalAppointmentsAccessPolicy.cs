namespace Application.Abstractions;

public interface IProfessionalAppointmentsAccessPolicy
{
    Task EnsureCanViewAsync(int agentId, int storeId, int professionalId, CancellationToken ct);
}