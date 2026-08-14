using System.Security.Claims;
using Application.Common.Contexts;

namespace Api.Common.Auth;

public sealed class HttpRequestContext : IRequestContext
{
    public int ActorId { get; }
    public bool IsUser { get; }
    public bool IsProfessional { get; }

    public HttpRequestContext(IHttpContextAccessor accessor)
    {
        var user = accessor.HttpContext?.User
            ?? throw new InvalidOperationException("No active HTTP request.");

        var actorIdClaim = user.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? throw new InvalidOperationException("Request is missing an actor id claim.");

        ActorId = int.Parse(actorIdClaim);

        var role = user.FindFirstValue(ClaimTypes.Role);

        IsUser = role == "User";
        IsProfessional = role == "Professional";
    }
}
