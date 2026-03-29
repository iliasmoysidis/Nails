using Application.Abstractions.Queries;
using Application.DTO.Appointment;

namespace Application.Features.Appointments.GetStoreAppointments;

public sealed class Handler
{
    private readonly IAppointmentQueries _queries;

    public Handler(
        IAppointmentQueries queries
    )
    {
        _queries = queries;
    }

    public async Task<IReadOnlyCollection<AppointmentListItemDTO>> Handle(Query query, CancellationToken ct)
    {
        return await _queries.GetStoreAppointmentsAsync(
            storeId: query.StoreId,
            from: query.From,
            to: query.To,
            ct: ct
        );
    }
}