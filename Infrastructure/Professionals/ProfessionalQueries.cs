using Application.Common.DTO;
using Application.Professionals.Common.Queries;
using Application.Professionals.GetDetails;
using Application.Professionals.Search;
using Infrastructure.Common;
using Infrastructure.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Professionals;

public sealed class ProfessionalQueries : IProfessionalQueries
{
    private readonly AppDbContext _context;

    public ProfessionalQueries(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ProfessionalDTO?> GetProfessionalDetailsAsync(int professionalId, CancellationToken ct)
    {
        return await (
            from professional in _context.Professionals
            where professional.Id == professionalId

            select new ProfessionalDTO(
                professional.Id,
                professional.FullName.ToString(),
                professional.Email.Value,
                professional.Phone.ToString()
            )
        ).FirstOrDefaultAsync(ct);
    }

    public async Task<PagedResult<ProfessionalSearchResultDTO>> SearchProfessionalsAsync(
        string? name,
        string? email,
        string? phone,
        int? page,
        int? limit,
        CancellationToken ct
    )
    {
        var query =
            from professional in _context.Professionals
            select professional;

        if (!string.IsNullOrWhiteSpace(name))
        {
            query = query.Where(p => EF.Functions.ILike(p.FullName.ToString(), $"%{name}%"));
        }

        if (!string.IsNullOrWhiteSpace(email))
        {
            query = query.Where(p => EF.Functions.ILike(p.Email.ToString(), $"%{email}%"));
        }

        if (!string.IsNullOrWhiteSpace(phone))
        {
            query = query.Where(p => EF.Functions.ILike(p.Phone.ToString(), $"%{phone}%"));
        }

        return await query
            .OrderBy(p => p.FullName.ToString())
            .Select(p =>
                new ProfessionalSearchResultDTO(
                    p.Id,
                    p.FullName.ToString(),
                    p.Email.ToString(),
                    p.Phone.ToString()
                )
            )
            .ToPagedResultAsync(page, limit, ct);
    }
}
