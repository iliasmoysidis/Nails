using System.Security.Cryptography;
using Domain.Common;
using Domain.Exceptions;

namespace Domain.Entities;

public class StoreProfessional : BaseEntity
{
    public int StoreId { get; private set; }
    public int ProfessionalId { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime? EndDate { get; private set; }

    public Store Store { get; private set; } = null!;
    public Professional Professional { get; private set; } = null!;

    private StoreProfessional()
    {
        StartDate = DateTime.UtcNow;
    }

    public static StoreProfessional Create(int storeId, int professionalId, DateTime? startDate = null)
    {
        return new StoreProfessional
        {
            StoreId = storeId,
            ProfessionalId = professionalId,
            StartDate = startDate ?? DateTime.UtcNow
        };
    }

    public void RemoveProfessional(DateTime? endDate = null)
    {
        if (EndDate != null)
        {
            throw new DomainException("This professional is already removed from the store.");
        }

        EndDate = endDate ?? DateTime.UtcNow;
        MarkAsUpdated();
    }
}
