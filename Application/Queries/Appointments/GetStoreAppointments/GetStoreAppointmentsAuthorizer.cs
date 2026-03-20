using Application.Abstractions.Authorization;
using Application.Abstractions.Queries;
using Application.Contexts;
using Application.Exceptions;

namespace Application.Queries.Appointments;

public sealed class GetStoreAppointmentsAuthorizer
    : IAuthorizer<GetStoreAppointmentsQuery>
{
    private readonly IRequestContext _context;
    private readonly IStaffQueries _queries;

    public GetStoreAppointmentsAuthorizer(
        IRequestContext context,
        IStaffQueries queries
    )
    {
        _context = context;
        _queries = queries;
    }

    public async Task AuthorizeAsync(
        GetStoreAppointmentsQuery request,
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