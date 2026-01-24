using Application.Contexts;
using Application.Exceptions;
using Domain.Entities;

namespace Application.Policies;

public sealed class AuthorizationPolicy
{
    public void EnsureIsStoreOwner(ActorContext actor)
    {
        if (!actor.IsOwner)
            throw new ApplicationLayerException("Only the store owner can perform this action.");
    }

    public void EnsureCanAccessProfessional(ActorContext actor, int professionalId)
    {
        EnsureOwnerOrSelf(actor, professionalId, "You are not allowed to access this professional's data.");
    }

    public void EnsureCanViewAppointment(ActorContext actor, Appointment appointment)
    {
        if (actor.UserId != appointment.UserId
            && actor.UserId != appointment.ProfessionalId
            && !actor.IsOwner)
            throw new ApplicationLayerException("You are not allowed to view this appointment.");
    }

    private static bool IsOwnerOrSelf(ActorContext actor, int subjectId)
        => actor.IsOwner || actor.UserId == subjectId;

    private static void EnsureOwnerOrSelf(
        ActorContext actor,
        int subjectId,
        string errorMessage
    )
    {
        if (!IsOwnerOrSelf(actor, subjectId))
            throw new ApplicationLayerException(errorMessage);
    }
}