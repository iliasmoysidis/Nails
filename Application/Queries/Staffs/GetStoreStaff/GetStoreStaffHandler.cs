using Application.Abstractions.Queries;
using Application.DTO.Staff;

namespace Application.Queries.Staffs;

public sealed class GetStoreStaffHandler
{
    private readonly IStaffQueries _queries;

    public GetStoreStaffHandler(IStaffQueries queries)
    {
        _queries = queries;
    }

    public async Task<IReadOnlyCollection<StaffMemberDTO>> Handle(GetStoreStaffQuery query, CancellationToken ct)
    {
        return await _queries.GetStoreStaffAsync(query.StoreId, ct);
    }
}