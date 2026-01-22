using Application.Abstractions;
using Application.DTO;
using Application.Repositories;

namespace Application.UseCases.Booking.Queries.GetProfessionalAppointments;

public sealed class GetProfessionalAppointmentsHandler : IQueryHandler<GetProfessionalAppointmentsQuery, IReadOnlyCollection<AppointmentListItemDTO>>
{
    private readonly IBookingReadRepository _repo;

    public GetProfessionalAppointmentsHandler(IBookingReadRepository repo)
    {
        _repo = repo;
    }

    public async Task<IReadOnlyCollection<AppointmentListItemDTO>> Handle(GetProfessionalAppointmentsQuery query, CancellationToken ct)
    {
        return await _repo.GetForProfessionalAsync(query.StoreId, query.ProfessionalId, query.Date, ct);
    }
}