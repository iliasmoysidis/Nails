using Domain.Entities;

namespace Domain.Repositories;

public interface IProfessionalAppointmentRepository
{
    ProfessionalAppointmentManager GetByProfessionalId(int professionalId);

    void SaveProfessionalAppointments(ProfessionalAppointmentManager manager);
}