using Application.Abstractions.Queries;
using Application.DTO.Staff;

namespace Application.Features.Staffs.GetProfessionalStores;

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