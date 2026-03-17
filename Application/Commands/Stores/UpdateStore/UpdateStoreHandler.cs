using Domain.Interfaces;
using Domain.ValueObjects.Identity;
using Domain.ValueObjects.Store;
using MediatR;

namespace Application.Commands.Stores;

public sealed class UpdateStoreHandler
    : IRequestHandler<UpdateStoreCommand>
{
    private readonly UpdateStoreContext _ctx;
    private readonly IClock _clock;

    public UpdateStoreHandler(
        UpdateStoreContext ctx,
        IClock clock)
    {
        _ctx = ctx;
        _clock = clock;
    }

    public Task Handle(UpdateStoreCommand command, CancellationToken ct)
    {
        _ctx.Store.UpdateDetails(
            clock: _clock,
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