using Domain.Entities;
using Domain.ValueObjects.Time;

namespace Domain.Repositories;

public interface IAppointmentRepository
{
    Task AddAsync(Appointment appointment);

    Task UpdateAsync(Appointment appointment);

    Task<Appointment?> GetByIdAsync(int appointmentId);

    Task<IReadOnlyCollection<Appointment>> GetByProfessionalAsync(
        int professionalId,
        UtcDateTime? date = null);

    Task<IReadOnlyCollection<Appointment>> GetByStoreAsync(
        int storeId,
        UtcDateTime? date = null);

    Task<IReadOnlyCollection<Appointment>> GetByUserAsync(
        int userId,
        UtcDateTime? date = null);
}
