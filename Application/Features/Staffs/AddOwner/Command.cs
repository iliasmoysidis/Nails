using MediatR;

namespace Application.Features.Staffs.AddOwner;

public sealed record Command(
    int StoreId,
    int ProfessionalId
) : IRequest;