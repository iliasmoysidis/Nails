using Domain.Entities;

namespace Application.Abstractions.Repositories;

public interface IAppointmentRepository
{
    Task<Appointment?> GetByIdAsync(int id, CancellationToken ct);
    Task AddAsync(Appointment appointment, CancellationToken ct);
}