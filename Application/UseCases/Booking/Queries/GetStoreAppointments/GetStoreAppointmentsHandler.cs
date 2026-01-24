using Application.Abstractions;
using Application.DTO;
using Application.Repositories;

namespace Application.UseCases.Booking.Queries.GetStoreAppointments;

public sealed class GetStoreAppointmentsHandler : IQueryHandler<GetStoreAppointmentsQuery, IReadOnlyCollection<AppointmentListItemDTO>>
{
    private readonly IBookingReadRepository _repo;
    private readonly IAuthorizationService _auth;


    public GetStoreAppointmentsHandler(
        IBookingReadRepository repo,
        IAuthorizationService auth
        )
    {
        _repo = repo;
        _auth = auth;
    }

    public async Task<IReadOnlyCollection<AppointmentListItemDTO>> Handle(GetStoreAppointmentsQuery query, CancellationToken ct)
    {
        await _auth.RequireStoreOwner(query.StoreId, ct);

        return await _repo.GetForStoreAsync(query.StoreId, query.Date, ct);
    }
}