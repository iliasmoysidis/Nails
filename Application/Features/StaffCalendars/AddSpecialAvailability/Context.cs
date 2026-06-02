using Domain.Entities;
using Domain.Services;

namespace Application.Features.StaffCalendars.AddSpecialAvailability;

public sealed class Context
{
    public ProfessionalAvailability ProfessionalAvailability { get; set; } = default!;
    public Staff Staff { get; set; } = default!;
}
