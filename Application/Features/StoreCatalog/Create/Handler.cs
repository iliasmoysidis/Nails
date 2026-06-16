using MediatR;

namespace Application.Features.Offerings.Create;

public sealed class Handler
    : IRequestHandler<Command, int>
{
    private readonly Context _ctx;

    public Handler(Context ctx)
    {
        _ctx = ctx;
    }

    public Task<int> Handle(Command command, CancellationToken ct)
    {
        var offering = _ctx.StoreOfferings.AddOffering(
            name: command.Name,
            price: command.Price,
            currency: command.Currency,
            durationMinutes: command.DurationMinutes,
            description: command.Description
        );

        return Task.FromResult(offering.Id);
    }
}
