using Application.Abstractions.Authorization;
using Application.Abstractions.Queries;
using Application.Contexts;
using Application.Exceptions;

namespace Application.Queries.StoreCalendars;

public sealed class GetStoreProfessionalCalendarAuthorizer
    : IAuthorizer<GetStoreProfessionalCalendarQuery>
{
    private readonly IRequestContext _context;
    private readonly IStaffQueries _queries;

    public GetStoreProfessionalCalendarAuthorizer(
        IRequestContext context,
        IStaffQueries queries
    )
    {
        _context = context;
        _queries = queries;
    }

    public async Task AuthorizeAsync(
        GetStoreProfessionalCalendarQuery request,
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