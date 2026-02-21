using Domain.Entities;

namespace Application.Abstractions.Repositories;

public interface IAppointmentRepository
{
    Task<Appointment?> GetByIdAsync(int id, CancellationToken ct);

    Task<IReadOnlyCollection<Appointment>> GetActiveProfessionalAsync(int professionalId, CancellationToken ct);

    Task<IReadOnlyCollection<Appointment>> GetUpcomingByStoreIdAsync(int storeId, CancellationToken ct);

    Task<IReadOnlyCollection<Appointment>> GetUpcomingByStoreIdAndProfessionalId(int storeId, int professionalId, CancellationToken ct);

    Task AddAsync(Appointment appointment, CancellationToken ct);
}