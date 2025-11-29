using Domain.Entities;

namespace Domain.Repositories;

public interface IProfessionalAppointmentRepository
{
    Task<ProfessionalAppointmentManager> GetByProfessionalId(int professionalId);

    Task SaveProfessionalAppointments(ProfessionalAppointmentManager manager);
}