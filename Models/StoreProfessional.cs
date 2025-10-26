namespace Nails.Models
{
    public class StoreProfessional
    {
        public int StoreId { get; set; }
        public int ProfessionalId { get; set; }
        public DateTime StartDate { get; set; } = DateTime.UtcNow;
        public DateTime? EndDate { get; set; } = null;

        public Store Store { get; set; } = null!;
        public User Professional { get; set; } = null!;
    }
}