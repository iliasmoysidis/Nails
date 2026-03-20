using Application.Abstractions.Queries;
using Application.DTO.Appointment;

namespace Application.Queries.Appointments;

public sealed class GetStoreAppointmentsHandler
{
    private readonly IAppointmentQueries _queries;

    public GetStoreAppointmentsHandler(
        IAppointmentQueries queries
    )
    {
        _queries = queries;
    }

    public async Task<IReadOnlyCollection<AppointmentListItemDTO>> Handle(GetStoreAppointmentsQuery query, CancellationToken ct)
    {
        return await _queries.GetStoreAppointmentsAsync(
            storeId: query.StoreId,
            from: query.From,
            to: query.To,
            ct: ct
        );
    }
}