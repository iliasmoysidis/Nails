using MediatR;

namespace Application.Appointments.Confirm;

public sealed record ConfirmAppointmentCommand(int AppointmentId) : IRequest;
