using Application.Abstractions;
using Application.DTO;
using Application.Exceptions;
using Application.Repositories;

namespace Application.UseCases.Booking.Queries.GetUserAppointments;

public sealed class GetUserAppointmentsHandler : IQueryHandler<GetUserAppointmentsQuery, IReadOnlyCollection<AppointmentListItemDTO>>
{
    private readonly IBookingReadRepository _repo;
    private readonly ICurrentUser _currentUser;

    public GetUserAppointmentsHandler(IBookingReadRepository repo, ICurrentUser currentUser)
    {
        _repo = repo;
        _currentUser = currentUser;
    }

    public async Task<IReadOnlyCollection<AppointmentListItemDTO>> Handle(GetUserAppointmentsQuery query, CancellationToken ct)
    {
        if (query.UserId != _currentUser.UserId)
            throw new ApplicationLayerException("You are not allowed to view these appointments.");

        return await _repo.GetForUserAsync(query.UserId, ct);
    }
}