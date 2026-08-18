using Application.Appointments.Common.DTO;
using Application.Appointments.Common.Queries;
using Application.Common.DTO;
using MediatR;

namespace Application.Appointments.GetStoreAppointments;

public sealed class GetStoreAppointmentsHandler
    : IRequestHandler<GetStoreAppointmentsQuery, PagedResult<AppointmentListItemDTO>>
{
    private readonly IAppointmentQueries _queries;

    public GetStoreAppointmentsHandler(
        IAppointmentQueries queries
    )
    {
        _queries = queries;
    }

    public async Task<PagedResult<AppointmentListItemDTO>> Handle(GetStoreAppointmentsQuery query, CancellationToken ct)
    {
        return await _queries.GetStoreAppointmentsAsync(
            storeId: query.StoreId,
            from: query.From,
            to: query.To,
            page: query.Page,
            limit: query.Limit,
            ct: ct
        );
    }
}
