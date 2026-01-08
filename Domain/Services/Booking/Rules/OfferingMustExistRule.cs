using Domain.Exceptions;
using Domain.Interfaces;
using Domain.ValueObjects.Time;

namespace Domain.Services.Booking.Rules;

public sealed class OfferingMustExistRule : IBookingRule
{
    public void EnsureSatisfied(Context ctx, int offeringId, int professionalId, UtcDateTime startAt, int? excludeAppointmentId = null)
    {
        if (ctx.StoreCatalog.GetOffering(offeringId) is null)
            throw new DomainException("Service not found");
    }
}