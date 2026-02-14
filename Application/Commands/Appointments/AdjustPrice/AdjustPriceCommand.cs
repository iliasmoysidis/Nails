using Application.DTO.Finance;

namespace Application.Commands.Appointments;

public sealed record AdjustPriceCommand(
    int AppointmentId,
    MoneyDTO Money,
    string Reason
);