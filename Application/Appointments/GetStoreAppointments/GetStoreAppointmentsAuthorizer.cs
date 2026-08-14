using Application.Common.Abstractions.Authorization;
using Application.Common.Contexts;
using Application.Common.Exceptions;
using Application.Roster.Common.Repositories;

namespace Application.Appointments.GetStoreAppointments;

public sealed class GetStoreAppointmentsAuthorizer
    : IAuthorizer<GetStoreAppointmentsQuery>
{
    private readonly IRequestContext _context;
    private readonly IStaffRepository _repo;

    public GetStoreAppointmentsAuthorizer(IRequestContext context, IStaffRepository repo)
    {
        _context = context;
        _repo = repo;
    }

    public async Task AuthorizeAsync(GetStoreAppointmentsQuery request, CancellationToken ct)
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
