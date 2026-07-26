using Application.Appointments.Common.DTO;
using Application.Appointments.Common.Queries;
using Application.Common.DTO;

namespace Application.Appointments.GetProfessionalAppointments;

public sealed class Handler
{
    private readonly IAppointmentQueries _queries;

    public Handler(
        IAppointmentQueries queries
    )
    {
        _queries = queries;
    }

    public async Task<PagedResult<AppointmentListItemDTO>> Handle(
        Query query,
        CancellationToken ct
    )
    {
        return await _queries.GetProfessionalAppointmentsAsync(
            professionalId: query.ProfessionalId,
            from: query.From,
            to: query.To,
            page: query.Page,
            limit: query.Limit,
            ct
        );
    }
}
