using Application.Appointments.Common.DTO;
using Application.Appointments.Common.Queries;
using Application.Common.DTO;

namespace Application.Appointments.GetUserAppointments;

public sealed class GetUserAppointmentsHandler
{
    private readonly IAppointmentQueries _queries;

    public GetUserAppointmentsHandler(IAppointmentQueries queries)
    {
        _queries = queries;
    }

    public async Task<PagedResult<AppointmentListItemDTO>> Handle(GetUserAppointmentsQuery query, CancellationToken ct)
    {
        return await _queries.GetUserAppointmentsAsync(
            userId: query.UserId,
            from: query.From,
            to: query.To,
            page: query.Page,
            limit: query.Limit,
            ct
        );
    }
}
