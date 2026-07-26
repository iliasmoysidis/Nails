using Application.Assignments.Common.Queries;
using Application.Assignments.GetProfessionalsByOffering;
using Application.Assignments.GetOfferingsByProfessional;
using Infrastructure.Common;
using Application.Common.DTO;
using Infrastructure.Common.Extensions;

namespace Infrastructure.Assignments;

public sealed class AssignmentRegistryQueries : IAssignmentRegistryQueries
{
    private readonly AppDbContext _context;

    public AssignmentRegistryQueries(AppDbContext context)
    {
        _context = context;
    }

    public async Task<PagedResult<ProfessionalSummaryDTO>> GetProfessionalsByOfferingAsync(
        int storeId,
        int offeringId,
        int? page,
        int? limit,
        CancellationToken ct
    )
    {
        return await (
            from registry in _context.AssignmentRegistries

            where registry.StoreId == storeId

            from assignment in registry.Assignments
                .Where(a => a.OfferingId == offeringId)

            join professional in _context.Professionals
                on assignment.ProfessionalId equals professional.Id

            select new ProfessionalSummaryDTO(
                professional.Id,
                professional.FullName.ToString(),
                professional.Email.Value,
                professional.Phone.ToString()
            )
        ).ToPagedResultAsync(page, limit, ct);
    }

    public async Task<PagedResult<OfferingSummaryDTO>> GetOfferingsByProfessionalAsync(
        int storeId,
        int professionalId,
        int? page,
        int? limit,
        CancellationToken ct
    )
    {
        return await (
            from registry in _context.AssignmentRegistries

            where registry.StoreId == storeId

            from assignment in registry.Assignments
                .Where(a => a.ProfessionalId == professionalId)

            join catalog in _context.StoreCatalogs
                on registry.StoreId equals catalog.StoreId

            from offering in catalog.Offerings
                .Where(o => o.Id == assignment.OfferingId)

            select new OfferingSummaryDTO(
                offering.Id,
                offering.Name.Value,
                offering.Price.Amount,
                offering.Price.Currency,
                offering.Duration.Minutes
            )
        ).ToPagedResultAsync(page, limit, ct);
    }
}
