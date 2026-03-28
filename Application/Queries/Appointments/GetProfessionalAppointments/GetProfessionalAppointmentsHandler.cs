using Application.Abstractions.Queries;
using Application.DTO.Appointment;

namespace Application.Queries.Appointments;

public sealed class GetProfessionalAppointmentsHandler
{
    private readonly IAppointmentQueries _queries;

    public GetProfessionalAppointmentsHandler(
        IAppointmentQueries queries
    )
    {
        _queries = queries;
    }

    public async Task<IReadOnlyCollection<AppointmentListItemDTO>> Handle(
        GetProfessionalAppointmentsQuery query,
        CancellationToken ct
    )
    {
        return await _queries.GetProfessionalAppointmentsAsync(
            professionalId: query.ProfessionalId,
            from: query.From,
            to: query.To,
            ct
        );
    }
}