namespace Nails.Models
{
    public class Store : BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string TaxIdentificationNumber { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Phone { get; set; } = null!;

        public ICollection<StoreManager> Managers { get; set; } = new List<StoreManager>();
        public ICollection<StoreProfessional> Staff { get; set; } = new List<StoreProfessional>();
        public ICollection<Service> Services { get; set; } = new List<Service>();

        public bool IsActive { get; set; } = true;
        public DateTime? DeletedAt { get; set; }
    }
}