using Application.Common.Abstractions.Authorization;
using Application.Common.Contexts;
using Application.Common.Exceptions;

namespace Application.Appointments.GetUserAppointments;

public sealed class GetUserAppointmentsAuthorizer
    : IAuthorizer<GetUserAppointmentsQuery>
{
    private readonly IRequestContext _context;

    public GetUserAppointmentsAuthorizer(IRequestContext context)
    {
        _context = context;
    }

    public Task AuthorizeAsync(GetUserAppointmentsQuery request, CancellationToken ct)
    {
        if (!_context.IsUser)
            throw new ApplicationLayerForbiddenException("User access required.");

        if (_context.ActorId != request.UserId)
            throw new ApplicationLayerForbiddenException("Cannot access other users.");

        return Task.CompletedTask;
    }
}