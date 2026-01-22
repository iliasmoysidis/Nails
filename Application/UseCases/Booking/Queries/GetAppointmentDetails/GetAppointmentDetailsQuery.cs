using Application.Abstractions;
using Application.DTO;

namespace Application.UseCases.Booking.Queries.GetAppointmentDetails;

public sealed record GetAppointmentDetailsQuery(int AppointmentId) : IQuery<AppointmentDetailsDTO?>;