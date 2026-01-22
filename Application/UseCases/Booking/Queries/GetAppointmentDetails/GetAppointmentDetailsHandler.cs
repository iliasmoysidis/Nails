using Application.Abstractions;
using Application.DTO;
using Application.Repositories;

namespace Application.UseCases.Booking.Queries.GetAppointmentDetails;

public sealed class GetAppointmentDetailsHandler : IQueryHandler<GetAppointmentDetailsQuery, AppointmentDetailsDTO?>
{
    private readonly IBookingReadRepository _repo;

    public GetAppointmentDetailsHandler(IBookingReadRepository repo)
    {
        _repo = repo;
    }

    public async Task<AppointmentDetailsDTO?> Handle(GetAppointmentDetailsQuery query, CancellationToken ct)
    {
        return await _repo.GetDetailsAsync(query.AppointmentId, ct);
    }
}