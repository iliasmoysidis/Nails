using Application.Abstractions.Repositories;
using Application.Abstractions.Validation.Users;
using Application.Commands.Users;
using Application.Exceptions;
using Domain.ValueObjects.Identity;

namespace Application.Validation.Users;

public sealed class RegistrationValidator : IRegistrationValidator
{
    private readonly IUserRepository _repo;

    public RegistrationValidator(IUserRepository repo)
    {
        _repo = repo;
    }

    public async Task EnsureUniqueAsync(RegisterCommand command, CancellationToken ct)
    {
        var email = Email.From(command.Email);

        var phone = Phone.From(
            countryCode: command.PhoneCountryCode,
            nationalNumber: command.PhoneNumber);

        if (await _repo.GetByEmailAsync(email, ct) is not null)
            throw new ApplicationLayerValidationException("Email is already registered.");

        if (await _repo.GetByPhoneAsync(phone, ct) is not null)
            throw new ApplicationLayerValidationException("Phone is already registered.");
    }
}