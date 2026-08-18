using MediatR;

namespace Application.Professionals.GetDetails;

public sealed record GetProfessionalDetailsQuery(int ProfessionalId) : IRequest<ProfessionalDTO>;