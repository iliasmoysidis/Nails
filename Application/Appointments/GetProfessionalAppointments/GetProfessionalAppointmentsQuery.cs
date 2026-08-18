using Application.Appointments.Common.DTO;
using Application.Common.DTO;
using MediatR;

namespace Application.Appointments.GetProfessionalAppointments;

public sealed record GetProfessionalAppointmentsQuery(
    int ProfessionalId,
    DateOnly From,
    DateOnly To,
    int? Page,
    int? Limit
) : IRequest<PagedResult<AppointmentListItemDTO>>;
