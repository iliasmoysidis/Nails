using Domain.Professionals.Services;

namespace Application.Professionals.Delete;

public sealed class DeleteProfessionalContext
{
    public ProfessionalDeletion ProfessionalDeletion { get; set; } = null!;
}
