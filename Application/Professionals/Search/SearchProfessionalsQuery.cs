using Application.Common.DTO;
using MediatR;

namespace Application.Professionals.Search;

public sealed record SearchProfessionalsQuery(
    string? Name,
    string? Email,
    string? Phone,
    int? Page,
    int? Limit
) : IRequest<PagedResult<ProfessionalSearchResultDTO>>;
