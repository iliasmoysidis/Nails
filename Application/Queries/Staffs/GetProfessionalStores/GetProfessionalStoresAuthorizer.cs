using Application.Abstractions.Authorization;
using Application.Contexts;
using Application.Exceptions;

namespace Application.Queries.Staffs;

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