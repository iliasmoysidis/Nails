using Domain.Interfaces;
using Domain.ValueObjects.Identity;
using MediatR;

namespace Application.Commands.Users;

public sealed class UpdateUserHandler
    : IRequestHandler<UpdateUserCommand>
{
    private readonly UpdateUserContext _ctx;
    private readonly IClock _clock;

    public UpdateUserHandler(
        UpdateUserContext ctx,
        IClock clock)
    {
        _ctx = ctx;
        _clock = clock;
    }

    public Task Handle(UpdateUserCommand command, CancellationToken ct)
    {
        _ctx.User.UpdatePersonalInfo(
            clock: _clock,
            fullName: ToFullName(command.FirstName, command.LastName),
            phone: ToPhone(command.PhoneCountryCode, command.PhoneNumber)
        );

        return Task.CompletedTask;
    }

    private static FullName? ToFullName(string? firstName, string? lastName)
        => firstName is null || lastName is null
            ? null
            : FullName.From(firstName, lastName);

    private static Phone? ToPhone(string? code, string? number)
        => code is null || number is null
            ? null
            : Phone.From(code, number);
}