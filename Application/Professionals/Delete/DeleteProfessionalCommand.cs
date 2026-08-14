using MediatR;

namespace Application.Professionals.Delete;

public sealed record DeleteProfessionalCommand(int ProfessionalId) : IRequest;