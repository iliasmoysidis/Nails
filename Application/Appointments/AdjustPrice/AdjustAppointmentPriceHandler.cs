using Domain.Common.ValueObjects;
using MediatR;

namespace Application.Appointments.AdjustPrice;

public sealed class AdjustAppointmentPriceHandler
    : IRequestHandler<AdjustAppointmentPriceCommand>
{
    private readonly AdjustAppointmentPriceContext _ctx;

    public AdjustAppointmentPriceHandler(AdjustAppointmentPriceContext ctx)
    {
        _ctx = ctx;
    }

    public Task Handle(AdjustAppointmentPriceCommand command, CancellationToken ct)
    {
        _ctx.Appointment.AdjustPrice(
            newPrice: ToMoney(command.Amount, command.Currency),
            reason: command.Reason
        );

        return Task.CompletedTask;
    }

    private static Money ToMoney(decimal amount, string currency)
        => Money.Create(amount, currency);
}