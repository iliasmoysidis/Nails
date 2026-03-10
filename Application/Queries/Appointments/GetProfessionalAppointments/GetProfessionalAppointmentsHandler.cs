using Application.Abstractions.Queries;
using Application.DTO.Appointment;
using Application.Guards;

namespace Application.Queries.Appointments;

public sealed class GetProfessionalAppointmentsHandler
{
    private readonly AuthorizationGuard _auth;
    private readonly IAppointmentQueries _queries;

    public GetProfessionalAppointmentsHandler(
        AuthorizationGuard auth,
        IAppointmentQueries queries
    )
    {
        _auth = auth;
        _queries = queries;
    }

    public async Task<IReadOnlyCollection<AppointmentListItemDTO>> Handle(GetProfessionalAppointmentsQuery query, CancellationToken ct)
    {
        _auth.EnsureProfessional();
        _auth.EnsureSelf(query.ProfessionalId);

        return await _queries.GetProfessionalAppointmentsAsync(
            professionalId: query.ProfessionalId,
            from: query.From,
            to: query.To,
            ct
        );
    }
}