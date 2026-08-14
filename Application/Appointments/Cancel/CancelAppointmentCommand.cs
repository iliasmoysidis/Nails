using MediatR;

namespace Application.Appointments.Cancel;

public sealed record CancelAppointmentCommand(
    int AppointmentId,
    string? Reason
) : IRequest;