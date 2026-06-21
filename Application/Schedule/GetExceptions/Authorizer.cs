using Application.Roster.Common.Queries;
using Application.Common.Abstractions.Authorization;
using Application.Common.Exceptions;

namespace Application.Schedule.GetExceptions;

public sealed class Authorizer
    : IAuthorizer<Query>
{
    private readonly IStaffQueries _queries;

    public Authorizer(IStaffQueries queries)
    {
        _queries = queries;
    }

    public async Task AuthorizeAsync(
        Query request,
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