using MediatR;

namespace Application.Appointments.Complete;

public sealed record CompleteAppointmentCommand(int AppointmentId) : IRequest;