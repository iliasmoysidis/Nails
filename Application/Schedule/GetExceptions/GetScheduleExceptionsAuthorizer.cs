using Application.Roster.Common.Repositories;
using Application.Common.Abstractions.Authorization;
using Application.Common.Exceptions;

namespace Application.Schedule.GetExceptions;

public sealed class GetScheduleExceptionsAuthorizer
    : IAuthorizer<GetScheduleExceptionsQuery>
{
    private readonly IStaffRepository _repo;

    public GetScheduleExceptionsAuthorizer(IStaffRepository repo)
    {
        _repo = repo;
    }

    public async Task AuthorizeAsync(
        GetScheduleExceptionsQuery request,
        CancellationToken ct
    )
    {
        var isStaff = await _repo.IsStaffMemberAsync(
            storeId: request.StoreId,
            professionalid: request.ProfessionalId,
            ct: ct
        );

        if (!isStaff)
            throw new ApplicationLayerNotFoundException("Staff access required.");
    }
}