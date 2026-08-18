using MediatR;

namespace Application.Appointments.GetDetails;

public sealed record GetAppointmentDetailsQuery(int AppointmentId) : IRequest<AppointmentDetailsDTO>;