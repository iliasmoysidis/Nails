using MediatR;

namespace Application.Commands.Appointments;

public sealed record MarkAppointmentAsNoShowCommand(int AppointmentId) : IRequest;