using MediatR;

namespace Application.Features.Professionals.Delete;

public sealed record Command(int ProfessionalId) : IRequest;