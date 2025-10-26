namespace Nails.Models
{
    public class Service
    {
        public int Id { get; set; }
        public int StoreId { get; set; }
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
        public TimeSpan Duration { get; set; }

        public Store Store { get; set; } = null!;
        public ICollection<ProfessionalService> Providers { get; set; } = new List<ProfessionalService>();
        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    }
}