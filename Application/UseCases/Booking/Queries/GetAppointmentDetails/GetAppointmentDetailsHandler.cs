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

    public Task<AppointmentDetailsDTO?> Handle(GetAppointmentDetailsQuery query, CancellationToken ct)
    {
        return _repo.GetDetailsAsync(query.AppointmentId, ct);
    }
}