using MediatR;

namespace Application.Features.Staffs.RemoveEmployee;

public sealed record Command(
    int StoreId,
    int ProfessionalId
) : IRequest;