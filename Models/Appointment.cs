using Nails.Enums;

namespace Nails.Models
{
    public class Appointment : BaseEntity
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int ServiceId { get; set; }
        public int ProfessionalId { get; set; }
        public DateTime StartAt { get; set; }
        public DateTime EndAt { get; set; }
        public decimal BookedPrice { get; set; }
        public string? Notes { get; set; }
        public AppointmentStatus Status { get; set; } = AppointmentStatus.PendingConfirmation;

        public User Customer { get; set; } = null!;
        public User Professional { get; set; } = null!;
        public Service Service { get; set; } = null!;
    }
}