namespace Nails.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int ServiceId { get; set; }
        public int ProfessionalId { get; set; }
        public DateTime StartAt { get; set; }
        public DateTime EndAt { get; set; }
        public decimal BookedPrice { get; set; }
        public string? Notes { get; set; }

        public Customer Customer { get; set; } = null!;
        public Professional Professional { get; set; } = null!;
        public Service Service { get; set; } = null!;
    }
}