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

    public void EnsureCanViewProfessionalAppointments(ActorContext actor, int professionalId)
    {
        if (!IsOwnerOrSelf(actor, professionalId))
            throw new ApplicationLayerException("You are not allowed to view this professional's appointments.");
    }

    public void EnsureCanViewExceptions(ActorContext actor, int professionalId)
    {
        if (!IsOwnerOrSelf(actor, professionalId))
            throw new ApplicationLayerException("You are not allowed to view this professional's exceptions.");
    }

    public void EnsureCanViewAppointment(ActorContext actor, Appointment appointment)
    {
        if (actor.UserId != appointment.UserId
            && actor.UserId != appointment.ProfessionalId
            && !actor.IsOwner)
            throw new ApplicationLayerException("You are not allowed to view this appointment.");
    }

    private bool IsOwnerOrSelf(ActorContext actor, int professionalId)
        => actor.IsOwner || actor.UserId == professionalId;
}