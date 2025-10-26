namespace Nails.Models
{
    public class Customer
    {
        public int UserId { get; set; }

        public User User { get; set; } = null!;
        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    }
}