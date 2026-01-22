using Application.Abstractions;
using Application.DTO;

namespace Application.UseCases.Booking.Queries.GetProfessionalAppointments;

public sealed record GetProfessionalAppointmentsQuery(int StoreId, int ProfessionalId, DateOnly Date) : IQuery<IReadOnlyCollection<AppointmentListItemDTO>>;