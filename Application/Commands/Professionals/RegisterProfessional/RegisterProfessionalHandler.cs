using Application.Abstractions.Repositories;
using Application.Exceptions;
using Domain.Entities;
using Domain.Interfaces;
using Domain.ValueObjects.Identity;

namespace Application.Commands.Professionals;

public sealed class RegisterProfessionalHandler
{
    private readonly IProfessionalRepository _repo;
    private readonly IClock _clock;

    public RegisterProfessionalHandler(
        IProfessionalRepository repo,
        IClock clock
    )
    {
        _repo = repo;
        _clock = clock;
    }

    public async Task<int> Handle(RegisterProfessionalCommand command, CancellationToken ct)
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


        if (await _repo.ExistsAsync(email, phone, taxIdNumber, ct))
            throw new ApplicationLayerValidationException("A professional with the same email, phone, or tax id already exists.");

        var professional = Professional.Create(
            fullName: fullName,
            email: email,
            phone: phone,
            taxIdNumber: taxIdNumber,
            clock: _clock
        );

        await _repo.AddAsync(professional, ct);

        return professional.Id;
    }
}