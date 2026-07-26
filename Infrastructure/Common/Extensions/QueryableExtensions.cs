using Application.Common.DTO;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Common.Extensions;

public static class QueryableExtensions
{
    public static async Task<PagedResult<T>> ToPagedResultAsync<T>(
        this IQueryable<T> query,
        int? page,
        int? limit,
        CancellationToken ct
    )
    {
        var totalCount = await query.CountAsync(ct);

        PaginationMetadata? pagination = null;

        if (page.HasValue && limit.HasValue)
        {
            var offset = (page.Value - 1) * limit.Value;
            query = query
                .Skip(offset)
                .Take(limit.Value);

            var totalPages = (int)Math.Ceiling((double)totalCount / limit.Value);

            pagination = new PaginationMetadata(
                Page: page.Value,
                Limit: limit.Value,
                TotalPages: totalPages,
                HasPreviousPage: page.Value > 1,
                HasNextPage: page.Value < totalPages
            );
        }

        var items = await query.ToListAsync(ct);

        return new PagedResult<T>(
            Items: items,
            TotalCount: totalCount,
            Pagination: pagination
        );
    }
}
