using Application.Abstractions.Repositories;
using Application.Abstractions.UnitOfWork;
using Application.Abstractions.Validation.Professionals;
using Domain.Entities;
using Domain.Interfaces;
using Domain.ValueObjects.Identity;

namespace Application.Commands.Professionals;

public sealed class RegisterHandler
{
    private readonly IRegistrationValidator _validator;
    private readonly IProfessionalRepository _repo;
    private readonly IClock _clock;
    private readonly IUnitOfWork _uow;

    public RegisterHandler(
        IRegistrationValidator validator,
        IProfessionalRepository repo,
        IClock clock,
        IUnitOfWork uow
    )
    {
        _validator = validator;
        _repo = repo;
        _clock = clock;
        _uow = uow;
    }

    public async Task<int> Handle(RegisterCommand command, CancellationToken ct)
    {
        await _validator.EnsureUniqueAsync(command, ct);

        var professional = CreateProfessional(command);

        await _repo.AddAsync(professional, ct);

        await _uow.SaveChangesAsync(ct);

        return professional.Id;
    }

    private Professional CreateProfessional(RegisterCommand command)
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

        var taxIdNumber = TaxIdentificationNumber.From(
            countryCode: command.TaxCountryCode,
            value: command.TaxIdNumber
        );

        var professional = Professional.Create(
            fullName: fullName,
            email: email,
            phone: phone,
            taxIdNumber: taxIdNumber,
            clock: _clock
        );

        return professional;
    }
}