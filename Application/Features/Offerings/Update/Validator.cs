using Application.Abstractions.Validation;
using Application.Guards;

namespace Application.Features.Offerings.Update;

public sealed class Validator
    : IRequestValidator<Command>
{
    private readonly ValidationGuard _val;
    private readonly Context _ctx;

    public Validator(
        ValidationGuard val,
        Context ctx)
    {
        _val = val;
        _ctx = ctx;
    }

    public Task ValidateAsync(Command command, CancellationToken ct)
    {
        _val.EnsureStoreOffersService(_ctx.Catalog, command.OfferingId);
        return Task.CompletedTask;
    }
}