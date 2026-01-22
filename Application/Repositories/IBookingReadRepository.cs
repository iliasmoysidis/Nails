using Application.DTO;

namespace Application.Repositories;

public interface IBookingReadRepository
{
    Task<AppointmentDetailsDTO?> GetDetailsAsync(int appointmentId, CancellationToken ct);
    Task<IReadOnlyCollection<AppointmentListItemDTO>> GetForUserAsync(int userId, CancellationToken ct);
    Task<IReadOnlyCollection<AppointmentListItemDTO>> GetForProfessionalAsync(int storeId, int professionalId, DateOnly? date, CancellationToken ct);
    Task<IReadOnlyCollection<AppointmentListItemDTO>> GetForStoreAsync(int storeId, DateOnly? date, CancellationToken ct);
}