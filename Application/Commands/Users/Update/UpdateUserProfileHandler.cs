using Application.Abstractions.Policies.Users;
using Application.Abstractions.Repositories;
using Application.Abstractions.UnitOfWork;
using Application.Exceptions;
using Domain.Interfaces;
using Domain.ValueObjects.Identity;

namespace Application.Commands.Users;

public sealed class UpdateUserProfileHandler
{
    private readonly IManageUserPolicy _policy;
    private readonly IUserRepository _repo;
    private readonly IClock _clock;
    private readonly IUnitOfWork _uow;

    public UpdateUserProfileHandler(
        IManageUserPolicy policy,
        IUserRepository repo,
        IClock clock,
        IUnitOfWork uow
    )
    {
        _policy = policy;
        _repo = repo;
        _clock = clock;
        _uow = uow;
    }

    public async Task Handle(UpdateUserProfileCommand command, CancellationToken ct)
    {
        _policy.EnsureCanManage(command.UserId);

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