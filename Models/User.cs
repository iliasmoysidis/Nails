namespace Nails.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Phone { get; set; } = null!;

        public Customer? Customer { get; set; }
        public Professional? Professional { get; set; }
        public ICollection<StoreOwner> OwnedStores { get; set; } = new List<StoreOwner>();
        public ICollection<StoreProfessional> Workplaces { get; set; } = new List<StoreProfessional>();

        public bool IsActive { get; set; } = true;
        public DateTime? DeletedAt { get; set; }
    }
}