using Application.Abstractions.Authorization;
using Application.Abstractions.Queries;
using Application.Contexts;
using Application.Exceptions;

namespace Application.Queries.StaffCalendars;

public sealed class GetStaffWeeklyScheduleAuthorizer
    : IAuthorizer<GetStaffWeeklyScheduleQuery>
{
    private IRequestContext _context;
    private IStaffQueries _queries;

    public GetStaffWeeklyScheduleAuthorizer(
        IRequestContext context,
        IStaffQueries queries
    )
    {
        _context = context;
        _queries = queries;
    }

    public async Task AuthorizeAsync(
        GetStaffWeeklyScheduleQuery request,
        CancellationToken ct
    )
    {
        var isStaff = await _queries.IsStaffMemberAsync(
            storeId: request.StoreId,
            professionalid: _context.ActorId,
            ct: ct
        );

        if (!isStaff)
            throw new ApplicationLayerForbiddenException("Staff access required.");
    }
}