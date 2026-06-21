using Application.Professionals.Common.Queries;
using Application.Common.Exceptions;

namespace Application.Professionals.GetDetails;

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