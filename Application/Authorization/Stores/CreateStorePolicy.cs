using Application.Abstractions.Policies.Stores;
using Application.Contexts;
using Application.Exceptions;

namespace Application.Authorization.Stores;

public sealed class CreateStorePolicy : ICreateStorePolicy
{
    private readonly IRequestContext _context;

    public CreateStorePolicy(IRequestContext context)
    {
        _context = context;
    }

    public void EnsureCanCreate()
    {
        if (!_context.IsProfessional) throw Forbidden();
    }

    public static ApplicationLayerForbiddenException Forbidden()
        => new ApplicationLayerForbiddenException("Only professionals can create stores.");
}