using Application.Roster.Common.Queries;

namespace Application.Roster.GetProfessionalStores;

public sealed class GetProfessionalStoresHandler
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