using MediatR;

namespace Application.Users.Register;

public sealed record Command(
    string FirstName,
    string LastName,
    string Email,
    string PhoneCountryCode,
    string PhoneNumber
) : IRequest<int>;