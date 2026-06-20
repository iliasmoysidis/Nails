using Domain.Assignments.ValueObjects;
using Domain.Common.Exceptions;

namespace Domain.Assignments;

public class AssignmentRegistry
{
    public int StoreId { get; }

    private readonly HashSet<Assignment> _assignments = new();

    private AssignmentRegistry() { }

    public AssignmentRegistry(int storeId)
    {
        StoreId = storeId;
    }

    internal void Add(int professionalId, int offeringId)
    {
        if (!_assignments.Add(new Assignment(professionalId, offeringId)))
            throw new InvariantException("Offering is already assigned to this professional.");
    }

    internal void Remove(int professionalId, int offeringId)
    {
        if (!_assignments.Remove(new Assignment(professionalId, offeringId)))
            throw new InvariantException("Offering is not assigned to the professional.");
    }

    public void Clear()
        => _assignments.Clear();

    public void RemoveByOffering(int offeringId)
        => _assignments.RemoveWhere(a => a.OfferingId == offeringId);

    public void RemoveByProfessional(int professionalId)
        => _assignments.RemoveWhere(a => a.ProfessionalId == professionalId);

    public bool IsAssigned(int professionalId, int offeringId)
        => _assignments.Contains(new Assignment(professionalId, offeringId));

    public IReadOnlyCollection<int> GetOfferingIds(int professionalId)
        => _assignments
            .Where(a => a.ProfessionalId == professionalId)
            .Select(a => a.OfferingId)
            .ToArray();
}
