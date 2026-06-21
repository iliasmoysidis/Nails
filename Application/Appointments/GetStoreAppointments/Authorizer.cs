using Application.Roster.Common.Queries;
using Application.Common.Abstractions.Authorization;
using Application.Common.Contexts;
using Application.Common.Exceptions;

namespace Application.Appointments.GetStoreAppointments;

public sealed class Authorizer
    : IAuthorizer<Query>
{
    private readonly IRequestContext _context;
    private readonly IStaffQueries _queries;

    public Authorizer(
        IRequestContext context,
        IStaffQueries queries
    )
    {
        _context = context;
        _queries = queries;
    }

    public async Task AuthorizeAsync(
        Query request,
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