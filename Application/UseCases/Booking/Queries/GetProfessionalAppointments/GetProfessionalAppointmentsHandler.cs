using Application.Abstractions;
using Application.DTO;
using Application.Policies.Interfaces;
using Application.Repositories;

namespace Application.UseCases.Booking.Queries.GetProfessionalAppointments;

public sealed class GetProfessionalAppointmentsHandler : IQueryHandler<GetProfessionalAppointmentsQuery, IReadOnlyCollection<AppointmentListItemDTO>>
{
    private readonly IBookingReadRepository _repo;
    private readonly IProfessionalAppointmentsAccessPolicy _policy;
    private readonly ICurrentUser _currentUser;

    public GetProfessionalAppointmentsHandler(
        IBookingReadRepository repo,
        IProfessionalAppointmentsAccessPolicy policy,
        ICurrentUser currentUser)
    {
        _repo = repo;
        _policy = policy;
        _currentUser = currentUser;
    }

    public async Task<IReadOnlyCollection<AppointmentListItemDTO>> Handle(GetProfessionalAppointmentsQuery query, CancellationToken ct)
    {
        await _policy.EnsureCanViewAsync(_currentUser.UserId, query.StoreId, query.ProfessionalId, ct);
        return await _repo.GetForProfessionalAsync(query.StoreId, query.ProfessionalId, query.Date, ct);
    }
}