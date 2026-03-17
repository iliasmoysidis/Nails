using Domain.Entities;

namespace Application.Commands.Professionals;

public sealed class UpdateProfessionalContext
{
    public Professional Professional { get; set; } = default!;
}