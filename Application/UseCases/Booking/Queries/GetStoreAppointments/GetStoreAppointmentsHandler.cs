using Application.Abstractions;
using Application.DTO;
using Application.Repositories;

namespace Application.UseCases.Booking.Queries.GetStoreAppointments;

public sealed class GetStoreAppointmentsHandler : IQueryHandler<GetStoreAppointmentsQuery, IReadOnlyCollection<AppointmentListItemDTO>>
{
    private readonly IBookingReadRepository _repo;

    public GetStoreAppointmentsHandler(IBookingReadRepository repo)
    {
        _repo = repo;
    }

    public async Task<IReadOnlyCollection<AppointmentListItemDTO>> Handle(GetStoreAppointmentsQuery query, CancellationToken ct)
    {
        return await _repo.GetForStoreAsync(query.StoreId, query.Date, ct);
    }
}