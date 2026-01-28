using Domain.ValueObjects.Time;

namespace Application.Abstractions.Services;

public interface IAppointmentAvailabilityService
{
    Task EnsureAvailableAsync(
        int storeId,
        int professionalId,
        UtcDateTime startAt,
        UtcDateTime endAt,
        int? ignoreAppointmentId,
        CancellationToken ct
    );
}