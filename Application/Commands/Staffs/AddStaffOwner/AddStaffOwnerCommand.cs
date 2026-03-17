using MediatR;

namespace Application.Commands.Staffs;

public sealed record AddStaffOwnerCommand(
    int StoreId,
    int ProfessionalId
) : IRequest;