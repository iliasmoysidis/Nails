using MediatR;

namespace Application.Professionals.Delete;

public sealed record Command(int ProfessionalId) : IRequest;