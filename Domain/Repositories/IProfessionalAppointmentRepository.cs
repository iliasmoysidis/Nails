using Domain.Entities;

namespace Domain.Repositories;

public interface IProfessionalAppointmentRepository
{
    Task<ProfessionalAppointments> GetByProfessionalAsync(int professionalId);

    Task SaveProfessionalAppointmentsAsync(ProfessionalAppointments appointments);
}