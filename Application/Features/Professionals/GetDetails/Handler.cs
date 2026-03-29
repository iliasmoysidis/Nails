using Application.Abstractions.Queries;
using Application.DTO.Professional;
using Application.Exceptions;

namespace Application.Features.Professionals.GetDetails;

public sealed class Handler
{
    private readonly IProfessionalQueries _queries;

    public Handler(IProfessionalQueries queries)
    {
        _queries = queries;
    }

    public async Task<ProfessionalDTO> Handle(Query query, CancellationToken ct)
    {
        return await _queries.GetProfessionalDetailsAsync(
            professionalId: query.ProfessionalId,
            ct: ct
        )
            ?? throw new ApplicationLayerNotFoundException("Professional not found.");
    }
}