using Application.Abstractions;
using Application.DTO;
using Application.Exceptions;
using Application.Repositories;

namespace Application.UseCases.Appointment.Queries.GetAppointmentDetails;

public sealed class GetAppointmentDetailsHandler : IQueryHandler<GetAppointmentDetailsQuery, AppointmentDetailsDTO?>
{
    private readonly IAppointmentReadRepository _repo;
    private readonly IAuthorizationService _auth;

    public GetAppointmentDetailsHandler(
        IAppointmentReadRepository repo,
        IAuthorizationService auth
        )
    {
        _repo = repo;
        _auth = auth;
    }

    public async Task<AppointmentDetailsDTO?> Handle(GetAppointmentDetailsQuery query, CancellationToken ct)
    {
        var appointment = await _repo.GetAppointmentAsync(query.AppointmentId, ct)
            ?? throw new ApplicationLayerException("Appointment not found.");

        await _auth.RequireAppointmentAccess(appointment.StoreId, appointment, ct);

        return await _repo.GetDetailsAsync(query.AppointmentId, ct);
    }
}