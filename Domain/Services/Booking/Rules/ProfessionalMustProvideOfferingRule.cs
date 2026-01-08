using Domain.Exceptions;
using Domain.Interfaces;
using Domain.ValueObjects.Time;

namespace Domain.Services.Booking.Rules;

public sealed class ProfessionalMustProvideOfferingRule : IBookingRule
{
    public void EnsureSatisfied(BookingContext ctx, int offeringId, int professionalId, UtcDateTime startAt, int? excludeAppointmentId = null)
    {
        if (!ctx.StoreCatalog.IsOfferingProvidedByProfessional(professionalId, offeringId))
            throw new DomainException("The selected professional does not provide the requested service.");
    }
}