using Domain.Entities;

namespace Application.Features.Appointments.Create;

public sealed class Context
{
    public StoreCatalog StoreCatalog { get; set; } = default!;
    public Offering Offering { get; set; } = default!;
    public StoreCalendar StoreCalendar { get; set; } = default!;
    public StaffCalendar StaffCalendar { get; set; } = default!;
    public Domain.Entities.Assignments Assignments { get; set; } = default!;
    public IReadOnlyCollection<Appointment> Appointments { get; set; } = default!;
}