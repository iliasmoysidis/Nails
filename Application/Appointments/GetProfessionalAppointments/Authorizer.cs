using Application.Common.Abstractions.Authorization;
using Application.Common.Contexts;
using Application.Common.Exceptions;

namespace Application.Appointments.GetProfessionalAppointments;

public sealed class Authorizer
    : IAuthorizer<Query>
{
    private readonly IRequestContext _context;

    public Authorizer(IRequestContext context)
    {
        _context = context;
    }

    public Task AuthorizeAsync(
        Query request,
        CancellationToken ct
    )
    {
        if (!_context.IsProfessional)
            throw new ApplicationLayerForbiddenException("Professional access required.");

        if (_context.ActorId != request.ProfessionalId)
            throw new ApplicationLayerForbiddenException("Cannot access other professionals.");

        return Task.CompletedTask;
    }
}