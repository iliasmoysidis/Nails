using Domain.Interfaces;

namespace Application.Abstractions.Services;

public interface IAppointmentCancellationService
{
    Task CancelUpcomingForStoreAsync(
        int storeId,
        IClock clock,
        CancellationToken ct
    );
}