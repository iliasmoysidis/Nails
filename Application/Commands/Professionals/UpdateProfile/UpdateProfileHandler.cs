using Application.Abstractions.Repositories;
using Application.Abstractions.UnitOfWork;
using Application.Guards;
using Application.Exceptions;
using Domain.Interfaces;
using Domain.ValueObjects.Identity;

namespace Application.Commands.Professionals;

public sealed class UpdateProfileHandler
{
    private readonly AuthorizationGuard _auth;
    private readonly IProfessionalRepository _repo;
    private readonly IClock _clock;
    private readonly IUnitOfWork _uow;

    public UpdateProfileHandler(
        AuthorizationGuard auth,
        IProfessionalRepository repo,
        IClock clock,
        IUnitOfWork uow
    )
    {
        _auth = auth;
        _repo = repo;
        _clock = clock;
        _uow = uow;
    }

    public async Task Handle(UpdateProfileCommand command, CancellationToken ct)
    {
        _auth.EnsureProfessional();
        _auth.EnsureSelf(command.ProfessionalId);

        var professional = await _repo.GetByIdAsync(command.ProfessionalId, ct)
            ?? throw new ApplicationLayerNotFoundException("Professional not found.");

        professional.UpdatePersonalInfo(
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