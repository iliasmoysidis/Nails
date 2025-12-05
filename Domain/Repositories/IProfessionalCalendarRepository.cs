using Domain.Entities;

namespace Domain.Repositories;

public interface IProfessionalCalendarRepository
{
    Task<ProfessionalCalendar> GetByProfessionalAsync(int professionalId);
}