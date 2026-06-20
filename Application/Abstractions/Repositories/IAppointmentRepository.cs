using Domain.Appointments;

namespace Application.Abstractions.Repositories;

public interface IAppointmentRepository
{
    Task<Appointment?> GetByIdAsync(int id, CancellationToken ct);

    Task<IReadOnlyCollection<Appointment>> GetByProfessionalIdAsync(int professionalId, CancellationToken ct);

    Task<IReadOnlyCollection<Appointment>> GetByUserIdAsync(int userId, CancellationToken ct);

    Task<IReadOnlyCollection<Appointment>> GetUpcomingByStoreIdAsync(int storeId, CancellationToken ct);

    Task<IReadOnlyCollection<Appointment>> GetUpcomingByStoreIdAndProfessionalId(int storeId, int professionalId, CancellationToken ct);

    Task<IReadOnlyCollection<Appointment>> GetUpcomingByProfessionalIdAsync(int professionalId, CancellationToken ct);

    Task<IReadOnlyCollection<Appointment>> GetUpcomingByUserIdAsync(int userId, CancellationToken ct);

    Task AddAsync(Appointment appointment, CancellationToken ct);
}
