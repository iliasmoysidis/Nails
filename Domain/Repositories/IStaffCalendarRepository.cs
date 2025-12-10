using Domain.Entities;

namespace Domain.Repositories;

public interface IStaffCalendarRepository
{
    Task<StaffCalendar> GetByStoreAndProfessionalAsync(int storeId, int professionalId);
    Task<IReadOnlyCollection<StaffCalendar>> GetAllByProfessionalAsync(int professionalId);
    Task SaveAsync(StaffCalendar manager);
}