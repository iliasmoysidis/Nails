using Application.Appointments.Common.DTO;
using Application.Common.DTO;
using MediatR;

namespace Application.Appointments.GetStoreAppointments;

public sealed record GetStoreAppointmentsQuery(
    int StoreId,
    DateOnly From,
    DateOnly To,
    int? Page,
    int? Limit
) : IRequest<PagedResult<AppointmentListItemDTO>>;
