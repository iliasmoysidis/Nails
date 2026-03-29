using MediatR;

namespace Application.Features.Staffs.RemoveOwner;

public sealed record Command(
    int StoreId,
    int ProfessionalId
) : IRequest;