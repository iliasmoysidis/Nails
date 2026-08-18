using MediatR;

namespace Application.Users.GetDetails;

public sealed record GetUserDetailsQuery(int UserId) : IRequest<UserDTO>;
