using Domain.Entities;

namespace Domain.Repositories;

public interface IProfessionalAppointmentRepository
{
    Task<ProfessionalAppointmentManager> GetByProfessionalAsync(int professionalId);

    Task SaveProfessionalAppointmentsAsync(ProfessionalAppointmentManager manager);
}