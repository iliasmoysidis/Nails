using Application.Abstractions;
using Application.Contexts;
using Application.DTO;
using Application.Policies;
using Application.Repositories;

namespace Application.UseCases.Booking.Queries.GetStoreAppointments;

public sealed class GetStoreAppointmentsHandler : IQueryHandler<GetStoreAppointmentsQuery, IReadOnlyCollection<AppointmentListItemDTO>>
{
    private readonly IBookingReadRepository _repo;
    private readonly ActorContextFactory _factory;
    private readonly AuthorizationPolicy _policy;


    public GetStoreAppointmentsHandler(
        IBookingReadRepository repo,
        AuthorizationPolicy policy,
        ActorContextFactory factory)
    {
        _repo = repo;
        _factory = factory;
        _policy = policy;
    }

    public async Task<IReadOnlyCollection<AppointmentListItemDTO>> Handle(GetStoreAppointmentsQuery query, CancellationToken ct)
    {
        var actor = await _factory.CreateAsync(query.StoreId, ct);
        _policy.EnsureIsStoreOwner(actor);
        return await _repo.GetForStoreAsync(query.StoreId, query.Date, ct);
    }
}