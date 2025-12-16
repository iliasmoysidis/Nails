using Domain.Entities;

namespace Domain.Repositories;

public interface IProfessionalAgendaRepository
{
    Task<ProfessionalAgenda> GetAsync(int professionalId);
    Task SaveAsync(ProfessionalAgenda agenda);
}