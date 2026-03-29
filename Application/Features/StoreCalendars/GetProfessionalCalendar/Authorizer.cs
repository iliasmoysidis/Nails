using Application.Abstractions.Authorization;
using Application.Abstractions.Queries;
using Application.Contexts;
using Application.Exceptions;

namespace Application.Features.StoreCalendars.GetProfessionalCalendar;

public sealed class Authorizer
    : IAuthorizer<Query>
{
    private readonly IRequestContext _context;
    private readonly IStaffQueries _queries;

    public Authorizer(
        IRequestContext context,
        IStaffQueries queries
    )
    {
        _context = context;
        _queries = queries;
    }

    public async Task AuthorizeAsync(
        Query request,
        CancellationToken ct
    )
    {
        if (_context.ActorId == request.ProfessionalId)
            return;

        var isOwner = await _queries.IsOwnerAsync(
            storeId: request.StoreId,
            professionalid: _context.ActorId,
            ct: ct
        );

        if (!isOwner)
            throw new ApplicationLayerForbiddenException("Not allowed to view professional calendar.");
    }
}