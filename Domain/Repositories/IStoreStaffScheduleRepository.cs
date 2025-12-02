using Domain.Entities;

namespace Domain.Repositories;

public interface IStoreStaffScheduleRepository
{
    Task<StaffCalendar> GetByStoreAndProfessionalAsync(int storeId, int professionalId);
    Task SaveAsync(StaffCalendar manager);
}