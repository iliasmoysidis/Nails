using Application.Abstractions.Policies.Stores;
using Application.Contexts;
using Application.Exceptions;
using Domain.Entities;

namespace Application.Authorization.Stores;

public sealed class ManageStorePolicy : IManageStorePolicy
{
    private readonly IRequestContext _context;

    public ManageStorePolicy(IRequestContext context)
    {
        _context = context;
    }

    public void EnsureCanManage(Staff staff)
    {
        if (!_context.IsProfessional) throw Forbidden();

        if (!staff.IsOwner(_context.ActorId)) throw Forbidden();
    }

    private static ApplicationLayerForbiddenException Forbidden()
        => new("Only a store owner can manage store details.");
}