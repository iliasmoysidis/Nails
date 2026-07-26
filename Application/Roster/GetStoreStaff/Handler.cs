using Application.Common.DTO;
using Application.Roster.Common.Queries;

namespace Application.Roster.GetStoreStaff;

public sealed class Handler
{
    private readonly IStaffQueries _queries;

    public Handler(IStaffQueries queries)
    {
        _queries = queries;
    }

    public async Task<PagedResult<StaffMemberDTO>> Handle(Query query, CancellationToken ct)
    {
        return await _queries.GetStoreStaffAsync(
            storeId: query.StoreId,
            page: query.Page,
            limit: query.Limit,
            ct: ct
        );
    }
}
