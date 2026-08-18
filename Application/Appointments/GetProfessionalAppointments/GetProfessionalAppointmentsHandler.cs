using Application.Appointments.Common.DTO;
using Application.Appointments.Common.Queries;
using Application.Common.DTO;
using MediatR;

namespace Application.Appointments.GetProfessionalAppointments;

public sealed class GetProfessionalAppointmentsHandler
    : IRequestHandler<GetProfessionalAppointmentsQuery, PagedResult<AppointmentListItemDTO>>
{
    private readonly IAppointmentQueries _queries;

    public GetProfessionalAppointmentsHandler(
        IAppointmentQueries queries
    )
    {
        _queries = queries;
    }

    public async Task<PagedResult<AppointmentListItemDTO>> Handle(
        GetProfessionalAppointmentsQuery query,
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
