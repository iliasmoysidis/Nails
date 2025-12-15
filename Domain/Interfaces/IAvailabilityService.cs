using Domain.ValueObjects.Time;

namespace Domain.Interfaces;

public interface IAvailabilityService
{
    Task EnsureStoreIsOpenAsync(int storeId, UtcDateTime startAt, UtcDateTime endAt);
    Task EnsureProfessionalIsAvailableAsync(int storeId, int professionalId, UtcDateTime startAt, UtcDateTime endAt);
}