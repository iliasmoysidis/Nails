using Application.Abstractions.Queries;
using Application.DTO.Appointment;

namespace Application.Queries.Appointments;

public sealed class GetUserAppointmentsHandler
{
    private readonly IAppointmentQueries _queries;

    public GetUserAppointmentsHandler(
        IAppointmentQueries queries
    )
    {
        _queries = queries;
    }

    public async Task<IReadOnlyCollection<AppointmentListItemDTO>> Handle(GetUserAppointmentsQuery query, CancellationToken ct)
    {
        return await _queries.GetUserAppointmentsAsync(
            userId: query.UserId,
            from: query.From,
            to: query.To,
            ct
        );
    }
}