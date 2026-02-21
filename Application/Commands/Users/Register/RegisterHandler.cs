using Application.Abstractions.Repositories;
using Application.Abstractions.UnitOfWork;
using Application.Abstractions.Validation.Users;
using Application.Exceptions;
using Domain.Entities;
using Domain.Interfaces;
using Domain.ValueObjects.Identity;

namespace Application.Commands.Users;

public sealed class RegisterHandler
{
    private readonly IRegistrationValidator _validator;
    private readonly IUserRepository _repo;
    private readonly IClock _clock;
    private readonly IUnitOfWork _uow;

    public RegisterHandler(
        IRegistrationValidator validator,
        IUserRepository repo,
        IClock clock,
        IUnitOfWork uow)
    {
        _validator = validator;
        _repo = repo;
        _clock = clock;
        _uow = uow;
    }

    public async Task<int> Handle(RegisterCommand command, CancellationToken ct)
    {
        await _validator.EnsureUniqueAsync(command, ct);

        var user = CreateUser(command);

        await _repo.AddAsync(user, ct);

        await _uow.SaveChangesAsync(ct);

        return user.Id;
    }

    private User CreateUser(RegisterCommand command)
    {
        var fullName = FullName.From(
            firstName: command.FirstName,
            lastName: command.LastName
        );

        var email = Email.From(command.Email);

        var phone = Phone.From(
            countryCode: command.PhoneCountryCode,
            nationalNumber: command.PhoneNumber);

        var user = User.Create(
            fullName: fullName,
            email: email,
            phone: phone,
            clock: _clock
        );

        return user;
    }
}