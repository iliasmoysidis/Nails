using Domain.Common.ValueObjects;
using MediatR;

namespace Application.Features.Appointments.AdjustPrice;

public sealed class Handler
    : IRequestHandler<Command>
{
    private readonly Context _ctx;

    public Handler(Context ctx)
    {
        _ctx = ctx;
    }

    public Task Handle(Command command, CancellationToken ct)
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