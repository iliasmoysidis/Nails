using MediatR;

namespace Application.Users.Update;

public sealed record UpdateUserCommand(
    int UserId,
    string? FirstName,
    string? LastName,
    string? PhoneCountryCode,
    string? PhoneNumber
) : IRequest;