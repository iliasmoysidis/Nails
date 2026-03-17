using Application.Abstractions.Repositories;
using Application.Exceptions;
using Domain.Entities;
using Domain.Interfaces;
using Domain.ValueObjects.Identity;
using MediatR;

namespace Application.Commands.Users;

public sealed class RegisterUserHandler
    : IRequestHandler<RegisterUserCommand, int>
{
    private readonly IUserRepository _repo;
    private readonly IClock _clock;

    public RegisterUserHandler(
        IUserRepository repo,
        IClock clock)
    {
        _repo = repo;
        _clock = clock;
    }

    public async Task<int> Handle(RegisterUserCommand command, CancellationToken ct)
    {
        var fullName = FullName.From(
            firstName: command.FirstName,
            lastName: command.LastName
        );

        var email = Email.From(command.Email);

        var phone = Phone.From(
            countryCode: command.PhoneCountryCode,
            nationalNumber: command.PhoneNumber);

        if (await _repo.ExistsAsync(email, phone, ct))
            throw new ApplicationLayerValidationException("User with the same email or phone is already registered.");

        var user = User.Create(
            fullName: fullName,
            email: email,
            phone: phone,
            clock: _clock
        );

        await _repo.AddAsync(user, ct);

        return user.Id;
    }
}