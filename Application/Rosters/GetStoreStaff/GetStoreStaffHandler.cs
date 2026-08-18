using Application.Common.DTO;
using Application.Rosters.Common.Queries;
using MediatR;

namespace Application.Rosters.GetStoreStaff;

public sealed class GetStoreStaffHandler
    : IRequestHandler<GetStoreStaffQuery, PagedResult<StaffMemberDTO>>
{
    private readonly IStaffQueries _queries;

    public GetStoreStaffHandler(IStaffQueries queries)
    {
        _queries = queries;
    }

    public async Task<PagedResult<StaffMemberDTO>> Handle(GetStoreStaffQuery query, CancellationToken ct)
    {
        return await _queries.GetStoreStaffAsync(
            storeId: query.StoreId,
            page: query.Page,
            limit: query.Limit,
            ct: ct
        );
    }
}
