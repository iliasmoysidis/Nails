namespace Domain.Interfaces;

public interface IAvailabilityService
{
    Task EnsureStoreIsOpenAsync(int storeId, DateTime startAt, DateTime endAt);
    Task EnsureProfessionalIsAvailableAsync(int storeId, int professionalId, DateTime startAt, DateTime endAt);
}