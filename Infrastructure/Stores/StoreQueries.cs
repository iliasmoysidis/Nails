using Application.Common.DTO;
using Application.Stores.Common.DTO;
using Application.Stores.Common.Queries;
using Application.Stores.GetDetails;
using Infrastructure.Common;
using Infrastructure.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Stores;

public sealed class StoreQueries : IStoreQueries
{
    private readonly AppDbContext _context;

    public StoreQueries(AppDbContext context)
    {
        _context = context;
    }

    public async Task<StoreDetailsDTO?> GetStoreDetailsAsync(int storeId, CancellationToken ct)
    {
        return await _context.Stores
            .Where(s => s.Id == storeId)
            .Select(
                s => new StoreDetailsDTO(
                    s.Id,
                    s.Name.Value,
                    s.Email.ToString(),
                    s.Phone.ToString(),
                    s.Address.ToString(),
                    s.TaxIdNumber.ToString()
                )
            ).FirstOrDefaultAsync(ct);
    }

    public async Task<PagedResult<StoreListItemDTO>> GetStoresAsync(
        int? page,
        int? limit,
        CancellationToken ct
    )
    {
        return await _context.Stores
            .OrderBy(s => s.Name.Value)
            .Select(
                s => new StoreListItemDTO(
                    s.Id,
                    s.Name.Value,
                    s.Address.City,
                    s.Address.CountryCode
                )
            )
            .ToPagedResultAsync(page, limit, ct);
    }

    public async Task<PagedResult<StoreListItemDTO>> SearchStoresAsync(
        string? name,
        string? city,
        string? countryCode,
        int? page,
        int? limit,
        CancellationToken ct
    )
    {
        var query = _context.Stores.AsQueryable();

        if (!string.IsNullOrWhiteSpace(name))
            query = query.Where(s => EF.Functions.ILike(s.Name.Value, $"%{name}%"));

        if (!string.IsNullOrWhiteSpace(city))
            query = query.Where(s => EF.Functions.ILike(s.Address.City, $"%{city}%"));

        if (!string.IsNullOrWhiteSpace(countryCode))
            query = query.Where(s => EF.Functions.ILike(s.Address.City, $"%{countryCode}%"));

        return await query
            .OrderBy(s => s.Name.Value)
            .Select(
                s => new StoreListItemDTO(
                    s.Id,
                    s.Name.Value,
                    s.Address.City,
                    s.Address.CountryCode
                )
            )
            .ToPagedResultAsync(page, limit, ct);
    }
}
