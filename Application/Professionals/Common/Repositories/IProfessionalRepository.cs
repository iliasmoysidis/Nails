using Domain.Professionals;
using Domain.Common.ValueObjects;

namespace Application.Professionals.Common.Repositories;

public interface IProfessionalRepository
{
    Task<Professional?> GetByIdAsync(int professionalId, CancellationToken ct);

    Task<bool> ExistsAsync(Email email, Phone phone, TaxIdentificationNumber taxIdNumber, CancellationToken ct);

    Task AddAsync(Professional professional, CancellationToken ct);
}
