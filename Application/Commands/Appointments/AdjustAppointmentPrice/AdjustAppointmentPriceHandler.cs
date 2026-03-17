using Domain.Interfaces;
using Domain.ValueObjects.Finance;
using MediatR;

namespace Application.Commands.Appointments;

public sealed class AdjustAppointmentPriceHandler
    : IRequestHandler<AdjustAppointmentPriceCommand>
{
    private readonly AdjustAppointmentPriceContext _ctx;
    private readonly IClock _clock;

    public AdjustAppointmentPriceHandler(
        AdjustAppointmentPriceContext ctx,
        IClock clock
    )
    {
        _ctx = ctx;
        _clock = clock;
    }

    public Task Handle(AdjustAppointmentPriceCommand command, CancellationToken ct)
    {
        _ctx.Appointment.AdjustPrice(
            newPrice: ToMoney(command.Amount, command.Currency),
            reason: command.Reason,
            clock: _clock
        );

        return Task.CompletedTask;
    }

    private static Money ToMoney(decimal amount, string currency)
        => Money.Create(amount, currency);
}