using Application.Abstractions;
using Application.DTO;
using Application.Repositories;

namespace Application.UseCases.Booking.Queries.GetUserAppointments;

public sealed class GetUserAppointmentsHandler : IQueryHandler<GetUserAppointmentsQuery, IReadOnlyCollection<AppointmentListItemDTO>>
{
    private readonly IBookingReadRepository _repo;

    public GetUserAppointmentsHandler(IBookingReadRepository repo)
    {
        _repo = repo;
    }

    public Task<IReadOnlyCollection<AppointmentListItemDTO>> Handle(GetUserAppointmentsQuery query, CancellationToken ct)
    {
        return _repo.GetForUserAsync(query.UserId, ct);
    }
}