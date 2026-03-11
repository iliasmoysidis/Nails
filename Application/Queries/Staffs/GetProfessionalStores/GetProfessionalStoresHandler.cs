using Application.Abstractions.Queries;
using Application.DTO.Staff;
using Application.Guards;

namespace Application.Queries.Staffs;

public sealed class GetProfessionalStoresHandler
{
    private readonly AuthorizationGuard _auth;
    private readonly IStaffQueries _queries;

    public GetProfessionalStoresHandler(AuthorizationGuard auth, IStaffQueries queries)
    {
        _auth = auth;
        _queries = queries;
    }

    public async Task<IReadOnlyCollection<ProfessionalStoreDTO>> Handle(GetProfessionalStoresQuery query, CancellationToken ct)
    {
        _auth.EnsureProfessional();
        _auth.EnsureSelf(query.ProfessionalId);

        return await _queries.GetProfessionalStoresAsync(query.ProfessionalId, ct);
    }
}