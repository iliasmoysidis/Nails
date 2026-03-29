using Application.Abstractions.Queries;
using Application.DTO.Staff;

namespace Application.Features.Staffs.GetStoreStaff;

public sealed class Handler
{
    private readonly IStaffQueries _queries;

    public Handler(IStaffQueries queries)
    {
        _queries = queries;
    }

    public async Task<IReadOnlyCollection<StaffMemberDTO>> Handle(Query query, CancellationToken ct)
    {
        return await _queries.GetStoreStaffAsync(query.StoreId, ct);
    }
}