using Application.Common.DTO;
using Application.Stores.Common.DTO;
using MediatR;

namespace Application.Stores.Search;

public sealed record SearchStoresQuery(
    string? Name,
    string? City,
    string? CountryCode,
    int? Page,
    int? Limit
) : IRequest<PagedResult<StoreListItemDTO>>;
