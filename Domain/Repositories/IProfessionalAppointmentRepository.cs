using Domain.Entities;

namespace Domain.Repositories;

public interface IProfessionalAppointmentRepository
{
    Task<ProfessionalAppointmentManager> GetByProfessionalIdAsync(int professionalId);

    Task SaveProfessionalAppointmentsAsync(ProfessionalAppointmentManager manager);
}