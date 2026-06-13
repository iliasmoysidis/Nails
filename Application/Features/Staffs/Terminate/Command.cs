using MediatR;

namespace Application.Features.Staffs.Terminate;

public sealed record Command(
    int StoreId,
    int ProfessionalId
) : IRequest;
