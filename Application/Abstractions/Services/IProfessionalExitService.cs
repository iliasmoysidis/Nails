namespace Application.Abstractions.Services;

public interface IProfessionalExitService
{
    Task LeaveStoreAsync(int storeId, int professionalId, CancellationToken ct);
}