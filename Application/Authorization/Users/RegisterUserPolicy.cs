using Application.Abstractions.Policies.Users;
using Application.Abstractions.Repositories;
using Application.Exceptions;
using Domain.ValueObjects.Identity;

namespace Application.Authorization.Users;

public sealed class RegisterUserPolicy : IRegisterUserPolicy
{
    private readonly IUserRepository _repo;

    public RegisterUserPolicy(IUserRepository repo)
    {
        _repo = repo;
    }

    public async Task EnsureCanRegisterAsync(Email email, Phone phone, CancellationToken ct)
    {
        if (await _repo.GetByEmailAsync(email, ct) is not null) throw Forbidden();

        if (await _repo.GetByPhoneAsync(phone, ct) is not null) throw Forbidden();
    }

    private static ApplicationLayerForbiddenException Forbidden()
        => new("Cannot create user.");
}