using MediatR;

namespace Application.Features.Staffs.Hire;

public sealed record Command(
    int StoreId,
    int ProfessionalId
) : IRequest;
