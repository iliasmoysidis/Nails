using MediatR;

namespace Application.Appointments.AdjustPrice;

public sealed record Command(
    int AppointmentId,
    decimal Amount,
    string Currency,
    string Reason
) : IRequest;