using Application.Abstractions.Repositories;
using Application.Abstractions.Validation.Professionals;
using Application.Commands.Professionals;
using Application.Exceptions;
using Domain.ValueObjects.Identity;

namespace Application.Validation.Professionals;

public sealed class RegistrationValidator : IRegistrationValidator
{
    private readonly IProfessionalRepository _repo;

    public RegistrationValidator(IProfessionalRepository repo)
    {
        _repo = repo;
    }

    public async Task EnsureUniqueAsync(RegisterCommand command, CancellationToken ct)
    {
        var email = Email.From(command.Email);

        var phone = Phone.From(
            countryCode: command.PhoneCountryCode,
            nationalNumber: command.PhoneNumber);

        var taxIdNumber = TaxIdentificationNumber.From(
            countryCode: command.TaxCountryCode,
            value: command.TaxIdNumber
        );

        if (await _repo.GetByEmailAsync(email, ct) is not null)
            throw new ApplicationLayerValidationException("Email is already registered.");

        if (await _repo.GetByPhoneAsync(phone, ct) is not null)
            throw new ApplicationLayerValidationException("Phone is already registered.");

        if (await _repo.GetByTaxIdAsync(taxIdNumber, ct) is not null)
            throw new ApplicationLayerValidationException("Tax id number is already registered.");
    }
}