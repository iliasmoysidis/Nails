using Domain.Entities;
using Domain.ValueObjects.Time;

namespace Domain.Repositories;

public interface IAppointmentReadRepository
{
    public Task<IReadOnlyCollection<Appointment>> GetByStoreAsync(int storeId, UtcDateTime? date = null);

    public Task<IReadOnlyCollection<Appointment>> GetByProfessionalAsync(int professionalId, UtcDateTime? date = null);

    public Task<IReadOnlyCollection<Appointment>> GetByUserAsync(int storeId, UtcDateTime? date = null);


}