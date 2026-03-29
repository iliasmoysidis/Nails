using MediatR;

namespace Application.Features.Professionals.LeaveStore;

public sealed record Command(
    int StoreId,
    int ProfessionalId
) : IRequest;