using Domain.Entities;

namespace Application.Commands.Professionals;

public sealed class DeleteProfessionalContext
{
    public Professional Professional { get; set; } = default!;
}