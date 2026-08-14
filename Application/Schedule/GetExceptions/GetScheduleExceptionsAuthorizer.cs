using Application.Roster.Common.Queries;
using Application.Common.Abstractions.Authorization;
using Application.Common.Exceptions;

namespace Application.Schedule.GetExceptions;

public sealed class GetScheduleExceptionsAuthorizer
    : IAuthorizer<GetScheduleExceptionsQuery>
{
    private readonly IStaffQueries _queries;

    public GetScheduleExceptionsAuthorizer(IStaffQueries queries)
    {
        _queries = queries;
    }

    public async Task AuthorizeAsync(
        GetScheduleExceptionsQuery request,
        CancellationToken ct
    )
    {
        var isStaff = await _queries.IsStaffMemberAsync(
            storeId: request.StoreId,
            professionalid: request.ProfessionalId,
            ct: ct
        );

        if (!isStaff)
            throw new ApplicationLayerNotFoundException("Staff access required.");
    }
}