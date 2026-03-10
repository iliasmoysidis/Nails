using Application.Abstractions.Queries;
using Application.DTO.Appointment;
using Application.Guards;

namespace Application.Queries.Appointments;

public sealed class GetUserAppointmentsHandler
{
    private readonly AuthorizationGuard _auth;
    private readonly IAppointmentQueries _queries;

    public GetUserAppointmentsHandler(
        AuthorizationGuard auth,
        IAppointmentQueries queries
    )
    {
        _auth = auth;
        _queries = queries;
    }

    public async Task<IReadOnlyCollection<AppointmentListItemDTO>> Handle(GetUserAppointmentsQuery query, CancellationToken ct)
    {
        _auth.EnsureUser();
        _auth.EnsureSelf(query.UserId);

        return await _queries.GetUserAppointmentsAsync(
            userId: query.UserId,
            from: query.From,
            to: query.To,
            ct
        );
    }
}