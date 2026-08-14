using MediatR;

namespace Application.Appointments.AdjustPrice;

public sealed record AdjustAppointmentPriceCommand(
    int AppointmentId,
    decimal Amount,
    string Currency,
    string Reason
) : IRequest;