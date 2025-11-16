using Domain.Common;
using Domain.Exceptions;

namespace Domain.Entities;

public class ProfessionalSchedule : BaseEntity
{
    public int Id { get; private set; }
    public int ProfessionalId { get; private set; }
    public int StoreId { get; private set; }
    public DayOfWeek DayOfWeek { get; private set; }
    public TimeSpan StartTime { get; private set; }
    public TimeSpan EndTime { get; private set; }
    public bool IsActive { get; private set; }

    public Professional Professional { get; private set; } = null!;
    public Store Store { get; private set; } = null!;

    private ProfessionalSchedule()
    {
        IsActive = true;
    }

    public static ProfessionalSchedule Create(int professionalId, int storeId, DayOfWeek dayOfWeek, TimeSpan startTime, TimeSpan endTime)
    {
        if (startTime >= endTime)
        {
            throw new DomainException("Start time must be before end time.");
        }

        return new ProfessionalSchedule
        {
            ProfessionalId = professionalId,
            StoreId = storeId,
            DayOfWeek = dayOfWeek,
            StartTime = startTime,
            EndTime = endTime,
            IsActive = true
        };
    }

    public void Activate()
    {
        if (StartTime >= EndTime)
        {
            throw new InvalidOperationException("Start time cannot be greater than or equal to end time.");
        }

        IsActive = true;
        MarkAsUpdated();
    }
    public void Deactivate()
    {
        if (!IsActive)
        {
            throw new DomainException("Schedule is already inactive.");
        }

        IsActive = false;
        MarkAsUpdated();
    }

    public void UpdateTimes(TimeSpan startTime, TimeSpan endTime)
    {
        if (!IsActive)
        {
            throw new DomainException("Cannot update inactive schedule.");
        }

        if (startTime >= endTime)
        {
            throw new DomainException("Start time must be before end time.");
        }

        StartTime = startTime;
        EndTime = endTime;
        MarkAsUpdated();
    }
}
