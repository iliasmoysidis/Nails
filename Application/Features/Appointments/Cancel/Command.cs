using MediatR;

namespace Application.Features.Appointments.Cancel;

public sealed record Command(
    int AppointmentId,
    string? Reason
) : IRequest;