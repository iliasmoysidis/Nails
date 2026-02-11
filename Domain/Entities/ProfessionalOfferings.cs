using Domain.Exceptions;
using Domain.ValueObjects.Offerings;

namespace Domain.Entities;

public class ProfessionalOfferings
{
    public int StoreId { get; }

    private readonly HashSet<ProfessionalOffering> _assignments = new();

    private ProfessionalOfferings() { }

    private ProfessionalOfferings(int storeId)
    {
        StoreId = storeId;
    }

    public static ProfessionalOfferings Create(int storeId)
        => new(storeId);

    public void Assign(int professionalId, int offeringId)
    {
        var assignment = new ProfessionalOffering(professionalId, offeringId);

        if (!_assignments.Add(assignment))
            throw new InvariantException("Offering is already assigned to this professional.");
    }

    public void Unassign(int professionalId, int offeringId)
    {
        var assignment = new ProfessionalOffering(professionalId, offeringId);

        if (!_assignments.Remove(assignment))
            throw new InvariantException("Offering is not assigned to the professional.");
    }

    public void Clear()
    {
        _assignments.Clear();
    }

    public void UnassignAllForOffering(int offeringId)
        => _assignments.RemoveWhere(a => a.OfferingId == offeringId);

    public void UnassignAllForProfessional(int professionalId)
        => _assignments.RemoveWhere(a => a.ProfessionalId == professionalId);

    public bool IsAssigned(int professionalId, int offeringId)
        => _assignments.Any(a =>
            a.ProfessionalId == professionalId &&
            a.OfferingId == offeringId);

    public IReadOnlyCollection<int> GetOfferingIds(int professionalId)
        => _assignments
            .Where(a => a.ProfessionalId == professionalId)
            .Select(a => a.OfferingId)
            .ToList()
            .AsReadOnly();
}