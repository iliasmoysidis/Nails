using Application.Abstractions;
using Application.DTO;
using Application.Policies.Interfaces;
using Application.Repositories;

namespace Application.UseCases.Booking.Queries.GetStoreAppointments;

public sealed class GetStoreAppointmentsHandler : IQueryHandler<GetStoreAppointmentsQuery, IReadOnlyCollection<AppointmentListItemDTO>>
{
    private readonly IBookingReadRepository _repo;
    private readonly IStoreOwnerAccessPolicy _policy;
    private readonly ICurrentUser _currentUser;

    public GetStoreAppointmentsHandler(IBookingReadRepository repo, IStoreOwnerAccessPolicy policy, ICurrentUser currentUser)
    {
        _repo = repo;
        _policy = policy;
        _currentUser = currentUser;
    }

    public async Task<IReadOnlyCollection<AppointmentListItemDTO>> Handle(GetStoreAppointmentsQuery query, CancellationToken ct)
    {
        await _policy.EnsureIsOwnerAsync(_currentUser.UserId, query.StoreId, ct);

        return await _repo.GetForStoreAsync(query.StoreId, query.Date, ct);
    }
}