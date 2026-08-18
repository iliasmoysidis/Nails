using Application.Users.Common.Queries;
using Application.Common.Exceptions;
using MediatR;

namespace Application.Users.GetDetails;

public sealed class GetUserDetailsHandler
    : IRequestHandler<GetUserDetailsQuery, UserDTO>
{
    private readonly IUserQueries _queries;

    public GetUserDetailsHandler(IUserQueries queries)
    {
        _queries = queries;
    }

    public async Task<UserDTO> Handle(GetUserDetailsQuery query, CancellationToken ct)
    {
        return await _queries.GetUserDetailsAsync(query.UserId, ct)
            ?? throw new ApplicationLayerNotFoundException("User not found.");
    }
}
