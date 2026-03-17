using MediatR;

namespace Application.Commands.Appointments;

public sealed record CompleteAppointmentCommand(int AppointmentId) : IRequest;