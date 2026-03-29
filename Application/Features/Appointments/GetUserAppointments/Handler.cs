using Application.Abstractions.Queries;
using Application.DTO.Appointment;

namespace Application.Features.Appointments.GetUserAppointments;

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
        return await _queries.GetUserAppointmentsAsync(
            userId: query.UserId,
            from: query.From,
            to: query.To,
            ct
        );
    }
}