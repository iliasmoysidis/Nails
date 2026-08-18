using Application.Common.DTO;
using Application.Professionals.Common.Queries;
using MediatR;

namespace Application.Professionals.Search;

public sealed class SearchProfessionalsHandler
    : IRequestHandler<SearchProfessionalsQuery, PagedResult<ProfessionalSearchResultDTO>>
{
    private readonly IProfessionalQueries _queries;

    public SearchProfessionalsHandler(IProfessionalQueries queries)
    {
        _queries = queries;
    }

    public async Task<PagedResult<ProfessionalSearchResultDTO>> Handle(SearchProfessionalsQuery query, CancellationToken ct)
    {
        return await _queries.SearchProfessionalsAsync(
            name: query.Name,
            email: query.Email,
            phone: query.Phone,
            page: query.Page,
            limit: query.Limit,
            ct: ct
        );
    }
}
