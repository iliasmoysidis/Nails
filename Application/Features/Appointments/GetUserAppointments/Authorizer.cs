using Application.Abstractions.Authorization;
using Application.Contexts;
using Application.Exceptions;

namespace Application.Features.Appointments.GetUserAppointments;

public sealed class Authorizer
    : IAuthorizer<Query>
{
    private readonly IRequestContext _context;

    public Authorizer(IRequestContext context)
    {
        _context = context;
    }

    public Task AuthorizeAsync(Query request, CancellationToken ct)
    {
        if (!_context.IsUser)
            throw new ApplicationLayerForbiddenException("User access required.");

        if (_context.ActorId != request.UserId)
            throw new ApplicationLayerForbiddenException("Cannot access other users.");

        return Task.CompletedTask;
    }
}