using MediatR;

namespace Application.Commands.Staffs;

public sealed record AddStaffEmployeeCommand(
    int StoreId,
    int ProfessionalId
) : IRequest;