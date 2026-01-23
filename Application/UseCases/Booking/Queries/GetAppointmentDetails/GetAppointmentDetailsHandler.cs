using Application.Abstractions;
using Application.DTO;
using Application.Exceptions;
using Application.Policies;
using Application.Repositories;

namespace Application.UseCases.Booking.Queries.GetAppointmentDetails;

public sealed class GetAppointmentDetailsHandler : IQueryHandler<GetAppointmentDetailsQuery, AppointmentDetailsDTO?>
{
    private readonly IBookingReadRepository _repo;
    private readonly ActorContextFactory _factory;
    private readonly AuthorizationPolicy _policy;

    public GetAppointmentDetailsHandler(
        IBookingReadRepository repo,
        AuthorizationPolicy policy,
        ActorContextFactory factory)
    {
        _repo = repo;
        _factory = factory;
        _policy = policy;
    }

    public async Task<AppointmentDetailsDTO?> Handle(GetAppointmentDetailsQuery query, CancellationToken ct)
    {
        var appointment = await _repo.GetAppointmentAsync(query.AppointmentId, ct)
            ?? throw new ApplicationLayerException("Appointment not found.");

        var actor = await _factory.CreateAsync(appointment.StoreId, ct);

        _policy.EnsureCanViewAppointment(actor, appointment);

        return await _repo.GetDetailsAsync(query.AppointmentId, ct);
    }
}