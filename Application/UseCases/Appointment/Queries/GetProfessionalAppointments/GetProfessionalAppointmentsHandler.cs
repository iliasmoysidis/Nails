using Application.Abstractions;
using Application.DTO;
using Application.Repositories;

namespace Application.UseCases.Appointment.Queries.GetProfessionalAppointments;

public sealed class GetProfessionalAppointmentsHandler : IQueryHandler<GetProfessionalAppointmentsQuery, IReadOnlyCollection<AppointmentListItemDTO>>
{
    private readonly IAppointmentReadRepository _repo;
    private readonly IAuthorizationService _auth;

    public GetProfessionalAppointmentsHandler(
        IAppointmentReadRepository repo,
        IAuthorizationService auth
        )
    {
        _repo = repo;
        _auth = auth;
    }

    public async Task<IReadOnlyCollection<AppointmentListItemDTO>> Handle(GetProfessionalAppointmentsQuery query, CancellationToken ct)
    {
        await _auth.RequireProfessionalAccess(query.StoreId, query.ProfessionalId, ct);

        return await _repo.GetForProfessionalAsync(query.StoreId, query.ProfessionalId, query.Date, ct);
    }
}