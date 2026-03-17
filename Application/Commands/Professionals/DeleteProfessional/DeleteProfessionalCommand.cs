using MediatR;

namespace Application.Commands.Professionals;

public sealed record DeleteProfessionalCommand(int ProfessionalId) : IRequest;