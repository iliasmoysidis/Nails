using MediatR;

namespace Application.Commands.Appointments;

public sealed record AdjustAppointmentPriceCommand(
    int AppointmentId,
    decimal Amount,
    string Currency,
    string Reason
) : IRequest;