using Domain.Entities;
using Domain.ValueObjects.Identity;

namespace Application.Abstractions.Repositories;

public interface IProfessionalRepository
{
    Task<Professional?> GetByIdAsync(int professionalId, CancellationToken ct);

    Task<Professional?> GetByEmailAsync(Email email, CancellationToken ct);

    Task<Professional?> GetByPhoneAsync(Phone phone, CancellationToken ct);

    Task<Professional?> GetByTaxIdAsync(TaxIdentificationNumber taxId, CancellationToken ct);

    Task AddAsync(Professional professional, CancellationToken ct);
}