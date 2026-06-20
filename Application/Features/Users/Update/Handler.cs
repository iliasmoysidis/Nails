using Domain.Users;
using Domain.Common;
using Domain.Common.ValueObjects;
using MediatR;

namespace Application.Features.Users.Update;

public sealed class Handler
    : IRequestHandler<Command>
{
    private readonly Context _ctx;
    private readonly IClock _clock;

    public Handler(
        Context ctx,
        IClock clock)
    {
        _ctx = ctx;
        _clock = clock;
    }

    public Task Handle(Command command, CancellationToken ct)
    {
        _ctx.User.UpdatePersonalInfo(
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