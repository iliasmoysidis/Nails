using Application.Abstractions.Validation;
using Application.Guards;

namespace Application.Commands.Assignments;

public sealed class AddAssignmentsValidator
    : IRequestValidator<AddAssignmentsCommand>
{
    private readonly ValidationGuard _val;
    private readonly AddAssignmentsContext _ctx;

    public AddAssignmentsValidator(
        ValidationGuard val,
        AddAssignmentsContext ctx)
    {
        _val = val;
        _ctx = ctx;
    }

    public Task ValidateAsync(AddAssignmentsCommand command, CancellationToken ct)
    {
        _val.EnsureProfessionalWorksForStore(_ctx.Staff, command.ProfessionalId);

        foreach (var offeringId in command.OfferingIds)
        {
            _val.EnsureStoreOffersService(_ctx.Catalog, offeringId);
        }

        return Task.CompletedTask;
    }
}