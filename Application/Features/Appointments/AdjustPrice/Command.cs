using MediatR;

namespace Application.Features.Appointments.AdjustPrice;

public sealed record Command(
    int AppointmentId,
    decimal Amount,
    string Currency,
    string Reason
) : IRequest;