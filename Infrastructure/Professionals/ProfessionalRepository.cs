using Application.Professionals.Common.Repositories;
using Infrastructure.Common;
using Domain.Professionals;
using Domain.Common.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Professionals;

public sealed class ProfessionalRepository : IProfessionalRepository
{
    private readonly AppDbContext _context;

    public ProfessionalRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Professional professional, CancellationToken ct)
    {
        await _context.Professionals.AddAsync(professional, ct);
    }

    public async Task<bool> ExistsAsync(Email email, Phone phone, TaxIdentificationNumber taxIdNumber, CancellationToken ct)
    {

        return await _context.Professionals.AnyAsync(p => p.Email == email ||
        p.Phone == phone ||
        p.TaxIdNumber == taxIdNumber,
        ct);
    }

    public async Task<Professional?> GetByIdAsync(int professionalId, CancellationToken ct)
    {
        return await _context.Professionals.FirstOrDefaultAsync(p => p.Id == professionalId, ct);
    }
}
