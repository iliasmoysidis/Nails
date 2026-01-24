using Application.Contexts;
using Domain.Entities;

namespace Application.Abstractions;

public interface IAuthorizationService
{
    Task<ActorContext> RequireStoreOwner(
        int storeId,
        CancellationToken ct
    );

    Task<ActorContext> RequireProfessionalAccess(
        int storeId,
        int professionalId,
        CancellationToken ct
    );

    Task<ActorContext> RequireAppointmentAccess(
        int storeId,
        Appointment appointment,
        CancellationToken ct
    );
}