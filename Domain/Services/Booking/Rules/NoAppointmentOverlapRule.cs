using Domain.Exceptions;
using Domain.ValueObjects.Time;

namespace Domain.Services.Booking.Rules;

public sealed class NoAppointmentOverlapRule
{
    public void EnsureSatisfied(Context ctx, int offeringId, int professionalId, UtcDateTime startAt, int? excludeAppointmentId = null)
    {
        var offering = ctx.StoreCatalog.GetOffering(offeringId)!;
        var endAt = startAt.Add(offering.Duration.Value);

        var hasConflict = ctx.Appointments.Any(a =>
            a.Id != excludeAppointmentId &&
            a.ConflictsWith(startAt, endAt)
        );

        if (hasConflict)
            throw new DomainException("Professional already has an appointment at this time.");
    }
}