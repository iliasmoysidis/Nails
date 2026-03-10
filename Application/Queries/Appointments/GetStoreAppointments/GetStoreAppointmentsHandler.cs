using Application.Abstractions.Queries;
using Application.Abstractions.Repositories;
using Application.DTO.Appointment;
using Application.Exceptions;
using Application.Guards;

namespace Application.Queries.Appointments;

public sealed class GetStoreAppointmentsHandler
{
    private readonly AuthorizationGuard _auth;
    private readonly IAppointmentQueries _queries;
    private readonly IStaffRepository _repo;

    public GetStoreAppointmentsHandler(
        AuthorizationGuard auth,
        IAppointmentQueries queries,
        IStaffRepository repo
    )
    {
        _auth = auth;
        _queries = queries;
        _repo = repo;
    }

    public async Task<IReadOnlyCollection<AppointmentListItemDTO>> Handle(GetStoreAppointmentsQuery query, CancellationToken ct)
    {
        var staff = await _repo.GetByStoreIdAsync(query.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Staff not found.");

        _auth.EnsureStaffMember(staff);

        return await _queries.GetStoreAppointmentsAsync(
            storeId: query.StoreId,
            from: query.From,
            to: query.To,
            ct: ct
        );
    }
}