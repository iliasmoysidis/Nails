using Application.Abstractions;
using Application.Contexts;
using Application.Policies;
using Domain.Entities;

namespace Application.Services;

public sealed class AuthorizationService : IAuthorizationService
{
    private readonly ActorContextFactory _factory;
    private readonly AuthorizationPolicy _policy;

    public AuthorizationService(
        ActorContextFactory factory,
        AuthorizationPolicy policy
    )
    {
        _factory = factory;
        _policy = policy;
    }

    public async Task<ActorContext> RequireStoreOwner(
        int storeId,
        CancellationToken ct
    )
    {
        var actor = await _factory.CreateAsync(storeId, ct);
        _policy.EnsureIsStoreOwner(actor);
        return actor;
    }

    public async Task<ActorContext> RequireProfessionalAccess(
        int storeId,
        int professionalId,
        CancellationToken ct
    )
    {
        var actor = await _factory.CreateAsync(storeId, ct);
        _policy.EnsureCanAccessProfessional(actor, professionalId);
        return actor;
    }

    public async Task<ActorContext> RequireAppointmentAccess(
        int storeId,
        Appointment appointment,
        CancellationToken ct
    )
    {
        var actor = await _factory.CreateAsync(storeId, ct);
        _policy.EnsureCanViewAppointment(actor, appointment);
        return actor;
    }
}