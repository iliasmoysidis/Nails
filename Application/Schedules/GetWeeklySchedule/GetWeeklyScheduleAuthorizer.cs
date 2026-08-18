using Application.Common.Abstractions.Authorization;
using Application.Common.Contexts;
using Application.Common.Exceptions;
using Application.Rosters.Common.Repositories;

namespace Application.Schedules.GetWeeklySchedule;

public sealed class GetWeeklyScheduleAuthorizer
    : IAuthorizer<GetWeeklyScheduleQuery>
{
    private IRequestContext _context;
    private IStaffRepository _repo;

    public GetWeeklyScheduleAuthorizer(IRequestContext context, IStaffRepository repo)
    {
        _context = context;
        _repo = repo;
    }

    public async Task AuthorizeAsync(GetWeeklyScheduleQuery request, CancellationToken ct)
    {
        var isStaff = await _repo.IsStaffMemberAsync(
            storeId: request.StoreId,
            professionalid: _context.ActorId,
            ct: ct
        );

        if (!isStaff)
            throw new ApplicationLayerForbiddenException("Staff access required.");
    }
}
