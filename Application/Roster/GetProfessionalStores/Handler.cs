using Application.Roster.Common.Queries;

namespace Application.Roster.GetProfessionalStores;

public sealed class Handler
{
    private readonly IStaffQueries _queries;

    public Handler(IStaffQueries queries)
    {
        _queries = queries;
    }

    public async Task<IReadOnlyCollection<ProfessionalStoreDTO>> Handle(Query query, CancellationToken ct)
    {
        return await _queries.GetProfessionalStoresAsync(query.ProfessionalId, ct);
    }
}