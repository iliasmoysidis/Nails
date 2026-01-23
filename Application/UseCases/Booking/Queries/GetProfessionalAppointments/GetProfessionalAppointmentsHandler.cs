using Application.Abstractions;
using Application.DTO;
using Application.Policies;
using Application.Repositories;

namespace Application.UseCases.Booking.Queries.GetProfessionalAppointments;

public sealed class GetProfessionalAppointmentsHandler : IQueryHandler<GetProfessionalAppointmentsQuery, IReadOnlyCollection<AppointmentListItemDTO>>
{
    private readonly IBookingReadRepository _repo;
    private readonly ActorContextFactory _factory;
    private readonly AuthorizationPolicy _policy;

    public GetProfessionalAppointmentsHandler(
        IBookingReadRepository repo,
        AuthorizationPolicy policy,
        ActorContextFactory factory)
    {
        _repo = repo;
        _factory = factory;
        _policy = policy;
    }

    public async Task<IReadOnlyCollection<AppointmentListItemDTO>> Handle(GetProfessionalAppointmentsQuery query, CancellationToken ct)
    {

        var actor = await _factory.CreateAsync(query.StoreId, ct);
        _policy.EnsureCanViewProfessionalAppointments(actor, query.ProfessionalId);
        return await _repo.GetForProfessionalAsync(query.StoreId, query.ProfessionalId, query.Date, ct);
    }
}