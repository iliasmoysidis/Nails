using Application.Abstractions.Queries;
using Application.DTO.User;
using Application.Exceptions;

namespace Application.Features.Users.GetProfile;

public sealed class Handler
{
    private readonly IUserQueries _queries;

    public Handler(IUserQueries queries)
    {
        _queries = queries;
    }

    public async Task<UserProfileDTO> Handle(Query query, CancellationToken ct)
    {
        var profile = await _queries.GetUserProfileAsync(query.UserId, ct)
            ?? throw new ApplicationLayerNotFoundException("User not found.");

        return profile;
    }
}