using Application.Abstractions.Policies.Users;
using Application.Abstractions.Repositories;
using Application.Abstractions.UnitOfWork;
using Application.Exceptions;
using Domain.Entities;
using Domain.Interfaces;
using Domain.ValueObjects.Identity;

namespace Application.Commands.Users;

public sealed class RegisterUserHandler
{
    private readonly IUserRepository _repo;
    private readonly IClock _clock;
    private readonly IUnitOfWork _uow;

    public RegisterUserHandler(
        IUserRepository repo,
        IClock clock,
        IUnitOfWork uow)
    {
        _repo = repo;
        _clock = clock;
        _uow = uow;
    }

    public async Task<int> Handle(RegisterUserCommand command, CancellationToken ct)
    {
        var email = Email.From(command.Email);
        var phone = Phone.From(
            countryCode: command.PhoneCountryCode,
            nationalNumber: command.PhoneNumber);
        var fullName = FullName.From(
            firstName: command.FirstName,
            lastName: command.LastName
        );

        if (await _repo.GetByEmailAsync(email, ct) is not null)
            throw new ApplicationLayerValidationException("Email is already registered.");
        if (await _repo.GetByPhoneAsync(phone, ct) is not null)
            throw new ApplicationLayerValidationException("Phone is already registered.");

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