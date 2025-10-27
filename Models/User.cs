namespace Nails.Models
{
    public class User : BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public bool IsCustomer { get; set; } = true;
        public bool IsProfessional { get; set; } = false;
        public string? TaxIdNumber { get; set; }

        public ICollection<Appointment> BookedAppointments { get; set; } = new List<Appointment>();
        public ICollection<StoreProfessional> Workplaces { get; set; } = new List<StoreProfessional>();
        public ICollection<ProfessionalService> ProvidedServices { get; set; } = new List<ProfessionalService>();
        public ICollection<Appointment> ProvidedAppointments { get; set; } = new List<Appointment>();
        public ICollection<ProfessionalSchedule> WorkSchedules { get; set; } = new List<ProfessionalSchedule>();
        public ICollection<ProfessionalTimeOff> TimeOffs { get; set; } = new List<ProfessionalTimeOff>();
        public ICollection<StoreManager> ManagedStores { get; set; } = new List<StoreManager>();


        public bool IsActive { get; set; } = true;
        public DateTime? DeletedAt { get; set; }
    }
}