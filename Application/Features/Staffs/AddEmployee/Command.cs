using MediatR;

namespace Application.Features.Staffs.AddEmployee;

public sealed record Command(
    int StoreId,
    int ProfessionalId
) : IRequest;