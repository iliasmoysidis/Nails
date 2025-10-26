namespace Nails.Models
{
    public class StoreProfessional
    {
        public int StoreId { get; set; }
        public int ProfessionalId { get; set; }

        public Store Store { get; set; } = null!;
        public Professional Professional { get; set; } = null!;
    }
}