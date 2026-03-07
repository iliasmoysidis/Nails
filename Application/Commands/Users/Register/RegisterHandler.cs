using Application.Abstractions.Repositories;
using Application.Abstractions.UnitOfWork;
using Application.Exceptions;
using Domain.Entities;
using Domain.Interfaces;
using Domain.ValueObjects.Identity;

namespace Application.Commands.Users;

public sealed class RegisterHandler
{
    private readonly IUserRepository _repo;
    private readonly IClock _clock;
    private readonly IUnitOfWork _uow;

    public RegisterHandler(
        IUserRepository repo,
        IClock clock,
        IUnitOfWork uow)
    {
        _repo = repo;
        _clock = clock;
        _uow = uow;
    }

    public async Task<int> Handle(RegisterCommand command, CancellationToken ct)
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
        await _uow.SaveChangesAsync(ct);

        return user.Id;
    }
}