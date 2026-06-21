using MediatR;

namespace Application.Appointments.Cancel;

public sealed record Command(
    int AppointmentId,
    string? Reason
) : IRequest;