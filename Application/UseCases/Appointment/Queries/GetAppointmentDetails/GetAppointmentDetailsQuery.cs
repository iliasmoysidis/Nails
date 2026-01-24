using Application.Abstractions;
using Application.DTO;

namespace Application.UseCases.Appointment.Queries.GetAppointmentDetails;

public sealed record GetAppointmentDetailsQuery(int AppointmentId) : IQuery<AppointmentDetailsDTO?>;