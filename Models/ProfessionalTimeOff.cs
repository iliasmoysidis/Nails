using Nails.Enums;

namespace Nails.Models
{
    public class ProfessionalTimeOff
    {
        public int Id { get; set; }
        public int ProfessionalId { get; set; }
        public int? StoreId { get; set; }
        public DateTime StartAt { get; set; }
        public DateTime EndAt { get; set; }
        public string? Reason { get; set; }
        public TimeOffType Type { get; set; }
        public User Professional { get; set; } = null!;
        public Store? Store { get; set; }
    }
}