using Domain.Interfaces;
using Domain.ValueObjects.Time;

namespace Domain.Services.Booking;

public sealed class RuleEngine
{
    private readonly IReadOnlyCollection<IBookingRule> _rules;

    public RuleEngine(IEnumerable<IBookingRule> rules)
    {
        _rules = rules.ToList().AsReadOnly();
    }

    public void EnsureAllSatisfied(
        Context ctx,
        int offeringId,
        int professionalId,
        UtcDateTime startAt,
        int? excludeAppointmentId = null
    )
    {
        foreach (var rule in _rules)
            rule.EnsureSatisfied(ctx, offeringId, professionalId, startAt, excludeAppointmentId);
    }
}