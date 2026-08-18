using Application.Rosters.Common.Queries;
using MediatR;

namespace Application.Rosters.GetProfessionalStores;

public sealed class GetProfessionalStoresHandler
    : IRequestHandler<GetProfessionalStoresQuery, IReadOnlyCollection<ProfessionalStoreDTO>>
{
    private readonly IStaffQueries _queries;

    public GetProfessionalStoresHandler(IStaffQueries queries)
    {
        _queries = queries;
    }

    public async Task<IReadOnlyCollection<ProfessionalStoreDTO>> Handle(GetProfessionalStoresQuery query, CancellationToken ct)
    {
        return await _queries.GetProfessionalStoresAsync(query.ProfessionalId, ct);
    }
}