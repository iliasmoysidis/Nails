using Application.Abstractions.Queries;
using Application.DTO.Appointment;

namespace Application.Features.Professionals.GetProfessionalAppointments;

public sealed class Handler
{
    private readonly IAppointmentQueries _queries;

    public Handler(
        IAppointmentQueries queries
    )
    {
        _queries = queries;
    }

    public async Task<IReadOnlyCollection<AppointmentListItemDTO>> Handle(
        Query query,
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