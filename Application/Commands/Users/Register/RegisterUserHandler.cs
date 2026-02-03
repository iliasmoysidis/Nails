using Application.Abstractions.Policies.Users;
using Application.Abstractions.Repositories;
using Application.Abstractions.UnitOfWork;
using Domain.Entities;
using Domain.Interfaces;
using Domain.ValueObjects.Identity;

namespace Application.Commands.Users;

public sealed class RegisterUserHandler
{
    private readonly IRegisterUserPolicy _policy;
    private readonly IUserRepository _repo;
    private readonly IClock _clock;
    private readonly IUnitOfWork _uow;

    public RegisterUserHandler(
        IRegisterUserPolicy policy,
        IUserRepository repo,
        IClock clock,
        IUnitOfWork uow)
    {
        _policy = policy;
        _repo = repo;
        _clock = clock;
        _uow = uow;
    }

    public async Task<int> Handle(RegisterUserCommand command, CancellationToken ct)
    {
        var email = Email.From(command.Email);
        var phone = Phone.From(countryCode: command.PhoneCountryCode, nationalNumber: command.PhoneNumber);

        await _policy.EnsureCanRegisterAsync(email, phone, ct);

        var user = User.Create(
            fullName: FullName.From(
                firstName: command.FirstName,
                lastName: command.LastName
            ),
            email: email,
            phone: phone,
            clock: _clock
        );

        await _repo.AddAsync(user, ct);
        await _uow.SaveChangesAsync(ct);

        return user.Id;
    }
}