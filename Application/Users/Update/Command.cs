using MediatR;

namespace Application.Users.Update;

public sealed record Command(
    int UserId,
    string? FirstName,
    string? LastName,
    string? PhoneCountryCode,
    string? PhoneNumber
) : IRequest;