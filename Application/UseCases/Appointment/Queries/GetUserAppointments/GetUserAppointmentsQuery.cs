using Application.Abstractions;
using Application.DTO;

namespace Application.UseCases.Appointment.Queries.GetUserAppointments;

public sealed record GetUserAppointmentsQuery(int UserId) : IQuery<IReadOnlyCollection<AppointmentListItemDTO>>;