using Domain.Common;
using Domain.Enums;

namespace Domain.Entities;

public class Appointment : BaseEntity
{
    public int CustomerId { get; private set; }
    public int ServiceId { get; private set; }
    public int ProfessionalId { get; private set; }
    public int StoreId { get; private set; }

    public DateTime StartAt { get; private set; }
    public DateTime EndAt { get; private set; }
    public decimal BookedPrice { get; private set; }
    public string? Notes { get; private set; }
    public AppointmentStatus Status { get; private set; } = AppointmentStatus.PendingConfirmation;
}
