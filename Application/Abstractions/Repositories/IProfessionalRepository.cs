using Domain.Entities;

namespace Application.Abstractions.Repositories;

public interface IProfessionalRepository
{
    Task<Professional?> GetByIdAsync(int professionalId, CancellationToken ct);

    Task AddAsync(Professional professional, CancellationToken ct);
}