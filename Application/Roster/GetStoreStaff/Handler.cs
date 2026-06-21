using Application.Roster.Common.Queries;

namespace Application.Roster.GetStoreStaff;

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