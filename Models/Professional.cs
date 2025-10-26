namespace Nails.Models
{
    public class Professional
    {
        public int UserId { get; set; }
        public string TaxIdentificationNumber { get; set; } = null!;

        public User User { get; set; } = null!;
        public ICollection<StoreProfessional> Employers { get; set; } = new List<StoreProfessional>();
        public ICollection<ProfessionalService> Services { get; set; } = new List<ProfessionalService>();
        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    }
}