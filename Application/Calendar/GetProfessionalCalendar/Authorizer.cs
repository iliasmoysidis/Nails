using Application.Roster.Common.Queries;
using Application.Common.Abstractions.Authorization;
using Application.Common.Contexts;
using Application.Common.Exceptions;

namespace Application.Calendar.GetProfessionalCalendar;

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