using Domain.Professionals;

namespace Application.Professionals.Update;

public sealed class UpdateProfessionalContext
{
    public Professional Professional { get; set; } = default!;
}