using MediatR;

namespace Application.Users.Register;

public sealed record RegisterUserCommand(
    string FirstName,
    string LastName,
    string Email,
    string PhoneCountryCode,
    string PhoneNumber
) : IRequest<int>;