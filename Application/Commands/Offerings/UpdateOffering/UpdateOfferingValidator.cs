using Application.Abstractions.Validation;
using Application.Guards;

namespace Application.Commands.Offerings;

public sealed class UpdateOfferingValidator
    : IRequestValidator<UpdateOfferingCommand>
{
    private readonly ValidationGuard _val;
    private readonly UpdateOfferingContext _ctx;

    public UpdateOfferingValidator(
        ValidationGuard val,
        UpdateOfferingContext ctx)
    {
        _val = val;
        _ctx = ctx;
    }

    public Task ValidateAsync(UpdateOfferingCommand command, CancellationToken ct)
    {
        _val.EnsureStoreOffersService(_ctx.Catalog, command.OfferingId);
        return Task.CompletedTask;
    }
}