using Domain.Common.ValueObjects;
using Domain.Stores.ValueObjects;
using MediatR;

namespace Application.Features.Stores.Update;

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
        _ctx.Store.UpdateDetails(
            name: ToName(command.Name),
            address: ToAddress(
                command.Street,
                command.City,
                command.PostalCode,
                command.State,
                command.CountryCode),
            phone: ToPhone(command.PhoneCountryCode, command.PhoneNumber)
        );

        return Task.CompletedTask;
    }

    private static StoreName? ToName(string? name)
        => name is null ? null : StoreName.Create(name);

    private static Address? ToAddress(
        string? street,
        string? city,
        string? postalCode,
        string? state,
        string? countryCode)
        => street is null ||
           city is null ||
           postalCode is null ||
           state is null ||
           countryCode is null
            ? null
            : Address.From(
                street,
                city,
                postalCode,
                state,
                countryCode
            );

    private static Phone? ToPhone(string? code, string? number)
        => code is null || number is null
            ? null
            : Phone.From(code, number);
}