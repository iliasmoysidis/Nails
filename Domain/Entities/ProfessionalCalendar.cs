namespace Domain.Entities;

public class ProfessionalCalendar
{
    public int ProfessionalId { get; private set; }

    private readonly List<StaffCalendar> _schedules = new();
    public IReadOnlyCollection<StaffCalendar> Schedules => _schedules.AsReadOnly();
}