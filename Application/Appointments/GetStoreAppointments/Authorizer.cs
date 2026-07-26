using Application.Common.Abstractions.Authorization;
using Application.Common.Contexts;
using Application.Common.Exceptions;
using Application.Roster.Common.Repositories;

namespace Application.Appointments.GetStoreAppointments;

public sealed class Authorizer
    : IAuthorizer<Query>
{
    private readonly IRequestContext _context;
    private readonly IStaffRepository _repo;

    public Authorizer(IRequestContext context, IStaffRepository repo)
    {
        _context = context;
        _repo = repo;
    }

    public async Task AuthorizeAsync(Query request, CancellationToken ct)
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
