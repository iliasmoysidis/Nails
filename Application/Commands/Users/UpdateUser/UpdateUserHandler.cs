using Application.Abstractions.Repositories;
using Application.Abstractions.UnitOfWork;
using Application.Guards;
using Application.Exceptions;
using Domain.Interfaces;
using Domain.ValueObjects.Identity;

namespace Application.Commands.Users;

public sealed class UpdateUserHandler
{
    private readonly AuthorizationGuard _auth;
    private readonly IUserRepository _repo;
    private readonly IClock _clock;
    private readonly IUnitOfWork _uow;

    public UpdateUserHandler(
        AuthorizationGuard auth,
        IUserRepository repo,
        IClock clock,
        IUnitOfWork uow
    )
    {
        _auth = auth;
        _repo = repo;
        _clock = clock;
        _uow = uow;
    }

    public async Task Handle(UpdateUserCommand command, CancellationToken ct)
    {
        _auth.EnsureUser();
        _auth.EnsureSelf(command.UserId);

        var user = await _repo.GetByIdAsync(command.UserId, ct)
            ?? throw new ApplicationLayerNotFoundException("User not found.");

        user.UpdatePersonalInfo(
            clock: _clock,
            fullName: ToFullName(command.FirstName, command.LastName),
            phone: ToPhone(command.PhoneCountryCode, command.PhoneNumber)
        );

        await _uow.SaveChangesAsync(ct);
    }

    private static FullName? ToFullName(string? firstName, string? lastName)
        => firstName is null || lastName is null ? null : FullName.From(firstName, lastName);

    private static Phone? ToPhone(string? code, string? number)
        => code is null || number is null ? null : Phone.From(code, number);
}