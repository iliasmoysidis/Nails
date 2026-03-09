using Domain.Exceptions;
using Domain.ValueObjects.Offerings;

namespace Domain.Entities;

public class Assignments
{
    public int StoreId { get; }

    private readonly HashSet<Assignment> _assignments = new();

    private Assignments() { }

    private Assignments(int storeId)
    {
        StoreId = storeId;
    }

    public static Assignments Create(int storeId)
        => new(storeId);

    public void Add(int professionalId, int offeringId)
    {
        var assignment = new Assignment(professionalId, offeringId);

        if (!_assignments.Add(assignment))
            throw new InvariantException("Offering is already assigned to this professional.");
    }

    public void Remove(int professionalId, int offeringId)
    {
        var assignment = new Assignment(professionalId, offeringId);

        if (!_assignments.Remove(assignment))
            throw new InvariantException("Offering is not assigned to the professional.");
    }

    public void Clear()
    {
        _assignments.Clear();
    }

    public void RemoveOfferingAssignments(int offeringId)
        => _assignments.RemoveWhere(a => a.OfferingId == offeringId);

    public void RemoveProfessionalAssignments(int professionalId)
        => _assignments.RemoveWhere(a => a.ProfessionalId == professionalId);

    public bool IsAssigned(int professionalId, int offeringId)
        => _assignments.Any(a =>
            a.ProfessionalId == professionalId &&
            a.OfferingId == offeringId);

    public IReadOnlyCollection<int> GetOfferingIds(int professionalId)
        => _assignments
            .Where(a => a.ProfessionalId == professionalId)
            .Select(a => a.OfferingId)
            .ToArray();
}