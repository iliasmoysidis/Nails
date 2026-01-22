using Application.Abstractions;
using Application.DTO;

namespace Application.UseCases.Booking.Queries.GetStoreAppointments;

public sealed record GetStoreAppointmentsQuery(int StoreId, DateOnly? Date) : IQuery<IReadOnlyCollection<AppointmentListItemDTO>>;