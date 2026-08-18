using Application.Common.Abstractions.Authorization;
using Application.Common.Contexts;
using Application.Common.Exceptions;

namespace Application.Rosters.GetProfessionalStores;

public sealed class GetProfessionalStoresAuthorizer
    : IAuthorizer<GetProfessionalStoresQuery>
{
    private readonly IRequestContext _context;

    public GetProfessionalStoresAuthorizer(IRequestContext context)
    {
        _context = context;
    }

    public Task AuthorizeAsync(
        GetProfessionalStoresQuery request,
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