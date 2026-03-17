using MediatR;

namespace Application.Commands.Appointments;

public sealed record ConfirmAppointmentCommand(int AppointmentId) : IRequest;