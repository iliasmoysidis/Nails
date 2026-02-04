using Application.Abstractions.Policies.Users;
using Application.Abstractions.Repositories;
using Application.Abstractions.UnitOfWork;
using Application.Exceptions;
using Domain.Interfaces;
using Domain.ValueObjects.Identity;

namespace Application.Commands.Users;

public sealed class UpdateUserProfileHandler
{
    private readonly IUpdateUserPolicy _policy;
    private readonly IUserRepository _repo;
    private readonly IClock _clock;
    private readonly IUnitOfWork _uow;

    public UpdateUserProfileHandler(
        IUpdateUserPolicy policy,
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
        await _policy.EnsureCanUpdateAsync(command.UserId, ct);

        var user = await _repo.GetByIdAsync(command.UserId, ct)
            ?? throw new ApplicationLayerNotFoundException("User not found.");

        FullName? fullName = (command.FirstName, command.LastName) switch
        {
            (null, null) => null,
            (string first, string last) => FullName.From(first, last),
            _ => throw new ApplicationLayerValidationException("Both first and last name must be provided together.")
        };

        Phone? phone = (command.PhoneCountryCode, command.PhoneNumber) switch
        {
            (null, null) => null,
            (string code, string number) => Phone.From(code, number),
            _ => throw new ApplicationLayerValidationException("Both phone country code and number must be provided together.")
        };

        user.UpdatePersonalInfo(_clock, fullName, phone);

        await _uow.SaveChangesAsync(ct);
    }
}