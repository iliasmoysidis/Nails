using Domain.Entities;

namespace Domain.Repositories;

public interface IAppointmentReadRepository
{
    public Task<IReadOnlyCollection<Appointment>> GetByStoreAsync(int storeId, DateTime? date = null);

    public Task<IReadOnlyCollection<Appointment>> GetByProfessionalAsync(int professionalId, DateTime? date = null);

    public Task<IReadOnlyCollection<Appointment>> GetByUserAsync(int storeId, DateTime? date = null);


}