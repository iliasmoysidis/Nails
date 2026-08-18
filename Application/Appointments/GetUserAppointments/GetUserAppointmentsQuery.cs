using Application.Appointments.Common.DTO;
using Application.Common.DTO;
using MediatR;

namespace Application.Appointments.GetUserAppointments;

public sealed record GetUserAppointmentsQuery(
    int UserId,
    DateOnly From,
    DateOnly To,
    int? Page,
    int? Limit
) : IRequest<PagedResult<AppointmentListItemDTO>>;
