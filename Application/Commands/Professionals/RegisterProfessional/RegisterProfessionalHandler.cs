using MediatR;
using Application.Abstractions.Repositories;
using Application.Exceptions;
using Domain.Entities;
using Domain.Interfaces;
using Domain.ValueObjects.Identity;

namespace Application.Commands.Professionals;

public sealed class RegisterProfessionalHandler
    : IRequestHandler<RegisterProfessionalCommand, int>
{
    private readonly IProfessionalRepository _repo;
    private readonly IClock _clock;

    public RegisterProfessionalHandler(
        IProfessionalRepository repo,
        IClock clock)
    {
        _repo = repo;
        _clock = clock;
    }

    public async Task<int> Handle(
        RegisterProfessionalCommand command,
        CancellationToken ct)
    {
        var fullName = ToFullName(command);
        var email = Email.From(command.Email);
        var phone = ToPhone(command);
        var taxId = ToTaxId(command);

        await EnsureNotExists(email, phone, taxId, ct);

        var professional = Professional.Create(
            fullName: fullName,
            email: email,
            phone: phone,
            taxIdNumber: taxId,
            clock: _clock
        );

        await _repo.AddAsync(professional, ct);

        return professional.Id;
    }

    private async Task EnsureNotExists(
        Email email,
        Phone phone,
        TaxIdentificationNumber taxId,
        CancellationToken ct)
    {
        if (await _repo.ExistsAsync(email, phone, taxId, ct))
            throw new ApplicationLayerValidationException(
                "A professional with the same email, phone, or tax id already exists.");
    }

    private static FullName ToFullName(RegisterProfessionalCommand cmd)
        => FullName.From(cmd.FirstName, cmd.LastName);

    private static Phone ToPhone(RegisterProfessionalCommand cmd)
        => Phone.From(cmd.PhoneCountryCode, cmd.PhoneNumber);

    private static TaxIdentificationNumber ToTaxId(RegisterProfessionalCommand cmd)
        => TaxIdentificationNumber.From(cmd.TaxCountryCode, cmd.TaxIdNumber);
}