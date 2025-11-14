using Domain.Common;
using Domain.Enums;

namespace Domain.Entities;

public class Appointment : HistoricEntity
{
    public int Id { get; private set; }
    public int UserId { get; private set; }
    public int ServiceId { get; private set; }
    public int ProfessionalId { get; private set; }
    public int StoreId { get; private set; }
    public DateTime StartAt { get; private set; }
    public DateTime EndAt { get; private set; }
    public decimal BookedPrice { get; private set; }
    public string? Notes { get; private set; }
    public AppointmentStatus Status { get; private set; }

    public User User { get; private set; } = null!;
    public Professional Professional { get; private set; } = null!;
    public Service Service { get; private set; } = null!;
    public Store Store { get; private set; } = null!;

    private Appointment()
    {
        Status = AppointmentStatus.PendingConfirmation;
    }
}
