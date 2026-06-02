using MediatR;

namespace Application.Features.Staffs.TerminateEmployment;

public sealed record Command(
    int StoreId,
    int ProfessionalId
) : IRequest;
