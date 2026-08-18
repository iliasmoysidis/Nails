using MediatR;

namespace Application.Rosters.AddOwner;

public sealed class AddStoreOwnerHandler
    : IRequestHandler<AddStoreOwnerCommand>
{
    private readonly AddStoreOwnerContext _ctx;

    public AddStoreOwnerHandler(AddStoreOwnerContext ctx)
    {
        _ctx = ctx;
    }

    public Task Handle(
        AddStoreOwnerCommand command,
        CancellationToken ct
    )
    {
        _ctx.Staff.AddOwner(command.ProfessionalId);

        return Task.CompletedTask;
    }
}