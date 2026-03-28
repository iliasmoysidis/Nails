using Application.Abstractions.Authorization;
using Application.Contexts;
using Application.Exceptions;
using Application.Queries.Appointments;

namespace Application.Queries.Professionals;

public sealed class GetProfessionalAppointmentsAuthorizer
    : IAuthorizer<GetProfessionalAppointmentsQuery>
{
    private readonly IRequestContext _context;

    public GetProfessionalAppointmentsAuthorizer(IRequestContext context)
    {
        _context = context;
    }

    public Task AuthorizeAsync(
        GetProfessionalAppointmentsQuery request,
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