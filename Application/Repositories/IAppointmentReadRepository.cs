using Application.DTO;
using Domain.Entities;

namespace Application.Repositories;

public interface IAppointmentReadRepository
{
    Task<Appointment?> GetAppointmentAsync(int appointmentId, CancellationToken ct);
    Task<AppointmentDetailsDTO?> GetDetailsAsync(int appointmentId, CancellationToken ct);
    Task<IReadOnlyCollection<AppointmentListItemDTO>> GetForUserAsync(int userId, CancellationToken ct);
    Task<IReadOnlyCollection<AppointmentListItemDTO>> GetForProfessionalAsync(int storeId, int professionalId, DateOnly? date, CancellationToken ct);
    Task<IReadOnlyCollection<AppointmentListItemDTO>> GetForStoreAsync(int storeId, DateOnly? date, CancellationToken ct);
}