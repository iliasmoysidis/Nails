using Application.Abstractions;
using Application.DTO;
using Application.Exceptions;
using Application.Repositories;

namespace Application.UseCases.Booking.Queries.GetAppointmentDetails;

public sealed class GetAppointmentDetailsHandler : IQueryHandler<GetAppointmentDetailsQuery, AppointmentDetailsDTO?>
{
    private readonly IBookingReadRepository _repo;
    private readonly IAppointmentVisibilityPolicy _policy;
    private readonly ICurrentUser _currentUser;

    public GetAppointmentDetailsHandler(IBookingReadRepository repo, IAppointmentVisibilityPolicy policy, ICurrentUser currentUser)
    {
        _repo = repo;
        _policy = policy;
        _currentUser = currentUser;
    }

    public async Task<AppointmentDetailsDTO?> Handle(GetAppointmentDetailsQuery query, CancellationToken ct)
    {
        var appointment = await _repo.GetAppointmentAsync(query.AppointmentId, ct)
            ?? throw new ApplicationLayerException("Appointment not found.");

        await _policy.EnsureCanViewAsync(appointment, _currentUser.UserId, ct);

        return await _repo.GetDetailsAsync(query.AppointmentId, ct);
    }
}