using Application.Professionals.Common.Repositories;
using Infrastructure.Common;
using Domain.Professionals;
using Domain.Common.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Professionals;

public sealed class ProfessionalRepository : IProfessionalRepository
{
    private readonly AppDbContext _db;

    public ProfessionalRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task AddAsync(Professional professional, CancellationToken ct)
    {
        await _db.Professionals.AddAsync(professional, ct);
    }

    public async Task<bool> DeleteAsync(int professionalId, CancellationToken ct)
    {
        var affectedRows = await _db.Professionals.Where(p => p.Id == professionalId)
            .ExecuteDeleteAsync(ct);
        return affectedRows > 0;
    }

    public async Task<bool> ExistsAsync(Email email, Phone phone, TaxIdentificationNumber taxIdNumber, CancellationToken ct)
    {

        return await _db.Professionals
        .AsNoTracking()
        .AnyAsync(
            p =>
            p.Email.Value == email.Value ||
            (
                p.Phone.CountryCode == phone.CountryCode &&
                p.Phone.Value == phone.Value
            ) ||
            (
                p.TaxIdNumber.CountryCode == taxIdNumber.CountryCode &&
                p.TaxIdNumber.Value == taxIdNumber.Value
            ),
        ct
        );
    }

    public async Task<Professional?> GetByIdAsync(int professionalId, CancellationToken ct)
    {
        return await _db.Professionals.FindAsync([professionalId], ct);
    }
}
