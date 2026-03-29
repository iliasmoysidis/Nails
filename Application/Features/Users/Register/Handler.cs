using Application.Abstractions.Repositories;
using Application.Exceptions;
using Domain.Entities;
using Domain.ValueObjects.Identity;
using MediatR;

namespace Application.Features.Users.Register;

public sealed class Handler
    : IRequestHandler<Command, int>
{
    private readonly IUserRepository _repo;

    public Handler(IUserRepository repo)
    {
        _repo = repo;
    }

    public async Task<int> Handle(Command command, CancellationToken ct)
    {
        var fullName = FullName.From(
            firstName: command.FirstName,
            lastName: command.LastName
        );

        var email = Email.From(command.Email);

        var phone = Phone.From(
            countryCode: command.PhoneCountryCode,
            nationalNumber: command.PhoneNumber
        );

        if (await _repo.ExistsAsync(email, phone, ct))
            throw new ApplicationLayerValidationException("User with the same email or phone is already registered.");

        var user = User.Create(
            fullName: fullName,
            email: email,
            phone: phone
        );

        await _repo.AddAsync(user, ct);

        return user.Id;
    }
}