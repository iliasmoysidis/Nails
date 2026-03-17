using Application.Abstractions.Validation;
using Application.Guards;

namespace Application.Commands.Assignments;

public sealed class RemoveAssignmentsValidator
    : IRequestValidator<RemoveAssignmentsCommand>
{
    private readonly ValidationGuard _val;
    private readonly RemoveAssignmentsContext _ctx;

    public RemoveAssignmentsValidator(
        ValidationGuard val,
        RemoveAssignmentsContext ctx)
    {
        _val = val;
        _ctx = ctx;
    }

    public Task ValidateAsync(RemoveAssignmentsCommand command, CancellationToken ct)
    {
        _val.EnsureProfessionalWorksForStore(_ctx.Staff, command.ProfessionalId);

        foreach (var offeringId in command.OfferingIds)
        {
            _val.EnsureStoreOffersService(_ctx.Catalog, offeringId);
        }

        return Task.CompletedTask;
    }
}