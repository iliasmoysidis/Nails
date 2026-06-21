using Application.Professionals.Common.Repositories;
using Domain.Professionals;
using MediatR;
using Application.Common.Exceptions;
using Domain.Common.ValueObjects;

namespace Application.Professionals.Register;

public sealed class Handler
    : IRequestHandler<Command, int>
{
    private readonly IProfessionalRepository _repo;

    public Handler(IProfessionalRepository repo)
    {
        _repo = repo;
    }

    public async Task<int> Handle(
        Command command,
        CancellationToken ct)
    {
        var fullName = ToFullName(command);
        var email = Email.From(command.Email);
        var phone = ToPhone(command);
        var taxId = ToTaxId(command);

        await EnsureNotExists(email, phone, taxId, ct);

        var professional = new Professional(
            fullName: fullName,
            email: email,
            phone: phone,
            taxIdNumber: taxId
        );

        await _repo.AddAsync(professional, ct);

        return professional.Id;
    }

    private async Task EnsureNotExists(
        Email email,
        Phone phone,
        TaxIdentificationNumber taxId,
        CancellationToken ct)
    {
        if (await _repo.ExistsAsync(email, phone, taxId, ct))
            throw new ApplicationLayerValidationException(
                "A professional with the same email, phone, or tax id already exists.");
    }

    private static FullName ToFullName(Command cmd)
        => FullName.From(cmd.FirstName, cmd.LastName);

    private static Phone ToPhone(Command cmd)
        => Phone.From(cmd.PhoneCountryCode, cmd.PhoneNumber);

    private static TaxIdentificationNumber ToTaxId(Command cmd)
        => TaxIdentificationNumber.From(cmd.TaxCountryCode, cmd.TaxIdNumber);
}
