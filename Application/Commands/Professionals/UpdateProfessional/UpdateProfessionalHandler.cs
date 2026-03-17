using Domain.Interfaces;
using Domain.ValueObjects.Identity;
using MediatR;

namespace Application.Commands.Professionals;

public sealed class UpdateProfessionalHandler
    : IRequestHandler<UpdateProfessionalCommand>
{
    private readonly UpdateProfessionalContext _ctx;
    private readonly IClock _clock;

    public UpdateProfessionalHandler(
        UpdateProfessionalContext ctx,
        IClock clock)
    {
        _ctx = ctx;
        _clock = clock;
    }

    public Task Handle(UpdateProfessionalCommand command, CancellationToken ct)
    {
        _ctx.Professional.UpdatePersonalInfo(
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